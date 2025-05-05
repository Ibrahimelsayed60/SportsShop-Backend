using MediatR;
using SportsShop.Core.Dtos;
using SportsShop.Core.Dtos.Orders;
using SportsShop.Core.Dtos.Products;
using SportsShop.Core.Dtos.ShoppingCart;
using SportsShop.Core.Entities;
using SportsShop.Core.Entities.Order;
using SportsShop.Core.Repositories.Contract;
using SportsShop.Service.CQRS.Products.Queries;
using SportsShop.Service.CQRS.ShoppingCarts.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsShop.Service.CQRS.Orders.Commands
{
    public record CreateOrderCommand(string email, CreateOrderDto CreateOrderDto):IRequest<ResultDto>;

    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, ResultDto>
    {
        private readonly IGenericRepository<Order> _orderRepo;
        private readonly IGenericRepository<DeliveryMethod> _deliveryMethodRepo;
        private readonly IGenericRepository<OrderItem> _orderItemRepo;
        private readonly IMediator _mediator;

        public CreateOrderCommandHandler(IGenericRepository<Order> orderRepo,
            IGenericRepository<DeliveryMethod> deliveryMethodRepo,
            IGenericRepository<OrderItem> orderItemRepo,
            IMediator mediator)
        {
            _orderRepo = orderRepo;
            _deliveryMethodRepo = deliveryMethodRepo;
            _orderItemRepo = orderItemRepo;
            _mediator = mediator;
        }

        public async Task<ResultDto> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var cart = (ShoppingCartDto)(await _mediator.Send(new GetShoppingCartQuery(request.CreateOrderDto.CartId))).Data;

            if (cart == null)
            {
                return ResultDto.Faliure("Card not found");
            }

            if(cart.PaymentIntentId == null)
            {
                return ResultDto.Faliure("No payment intent for this order");
            }

            var items = new List<OrderItem>();

            foreach (var item in cart.Items)
            {
                var productItem = (ProductDto)(await _mediator.Send(new GetProductByIdQuery(item.ProductId))).Data;

                if (productItem == null) 
                    return ResultDto.Faliure("Problem with the order");


                var itemOrdered = new ProductItemOrdered
                {
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    PictureUrl = item.PictureUrl
                };

                var orderItem = new OrderItem
                {
                    ItemOrdered = itemOrdered,
                    Price = productItem.Price,
                    Quantity = item.Quantity
                };
                orderItem = await _orderItemRepo.AddAsync(orderItem);
                items.Add(orderItem);
            }

            var deliveryMethod = await _deliveryMethodRepo.GetByIdAsync(request.CreateOrderDto.DeliveryMethodId);

            if (deliveryMethod == null) 
                return ResultDto.Faliure("No delivery method selected");

            

            var order = new Order
            {
                OrderItems = items,
                DeliveryMethod = deliveryMethod,
                ShippingAddress = request.CreateOrderDto.ShippingAddress,
                Subtotal = items.Sum(x => x.Price * x.Quantity),
                PaymentSummary = request.CreateOrderDto.PaymentSummary,
                PaymentIntentId = cart.PaymentIntentId,
                BuyerEmail = request.email
            };

            await _orderRepo.AddAsync(order);

            await _orderRepo.SaveChangesAsync();

            return ResultDto.Sucess(order);
        }
    }

}
