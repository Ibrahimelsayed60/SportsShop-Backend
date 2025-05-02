using MediatR;
using Microsoft.Extensions.Configuration;
using SportsShop.Core.Dtos;
using SportsShop.Core.Dtos.ShoppingCart;
using SportsShop.Core.Entities;
using SportsShop.Core.Repositories.Contract;
using SportsShop.Service.CQRS.ShoppingCarts.Commands;
using SportsShop.Service.CQRS.ShoppingCarts.Queries;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsShop.Service.CQRS.Payment.Commands
{
    public record CreateOrUpdatePaymentIntentCommand(string cartId) : IRequest<ResultDto?>;

    public class CreateOrUpdatePaymentIntentCommandHandler : IRequestHandler<CreateOrUpdatePaymentIntentCommand, ResultDto?>
    {
        private readonly IMediator _mediator;
        private readonly IConfiguration _config;
        private readonly IGenericRepository<Core.Entities.Product> _productRepo;
        private readonly IGenericRepository<DeliveryMethod> _deliveryRepo;

        public CreateOrUpdatePaymentIntentCommandHandler(
            IMediator mediator, 
            IConfiguration config,
            IGenericRepository<Core.Entities.Product> productRepo,
            IGenericRepository<DeliveryMethod> deliveryRepo)
        {
            _mediator = mediator;
            _config = config;
            _productRepo = productRepo;
            _deliveryRepo = deliveryRepo;
        }

        public async Task<ResultDto?> Handle(CreateOrUpdatePaymentIntentCommand request, CancellationToken cancellationToken)
        {
            StripeConfiguration.ApiKey = _config["StripeSettings:Secretkey"];

            var cart = await _mediator.Send(new GetShoppingCartQuery(request.cartId));

            if (cart == null) return null;

            var shippingPrice = 0m;

            if (((ShoppingCartDto)cart.Data).DeliveryMethodId.HasValue)
            {
                var deliveryMethod = await _deliveryRepo.GetByIdAsync((int)cart.Data.DeliveryMethodId);

                if (deliveryMethod == null) return null;

                shippingPrice = deliveryMethod.Price;
            }

            foreach (var item in cart.Data.Items)
            {
                var productItem = await _productRepo.GetByIdAsync(item.ProductId);

                if (productItem == null) return null;

                if (item.Price != productItem.Price)
                {
                    item.Price = productItem.Price;
                }
            }

            var service = new PaymentIntentService();
            PaymentIntent? intent = null;

            if (string.IsNullOrEmpty(cart.Data.PaymentIntentId))
            {
                var options = new PaymentIntentCreateOptions
                {
                    Amount = (long)((ShoppingCartDto)cart.Data).Items.Sum(x => x.Quantity * (x.Price * 100))
                        + (long)shippingPrice * 100,
                    Currency = "usd",
                    PaymentMethodTypes = ["card"]
                };
                intent = await service.CreateAsync(options);
                cart.Data.PaymentIntentId = intent.Id;
                cart.Data.ClientSecret = intent.ClientSecret;
            }
            else
            {
                var options = new PaymentIntentUpdateOptions
                {
                    Amount = (long)((ShoppingCartDto)cart.Data).Items.Sum(x => x.Quantity * (x.Price * 100))
                        + (long)shippingPrice * 100
                };
                intent = await service.UpdateAsync(cart.Data.PaymentIntentId, options);
            }

            cart = await _mediator.Send(new AddOrUpdateShoppingCartCommand(cart.Data));

            return cart;

        }
    }
}
