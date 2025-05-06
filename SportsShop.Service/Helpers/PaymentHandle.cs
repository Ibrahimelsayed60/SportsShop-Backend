using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SportsShop.Core.Entities.Order;
using SportsShop.Core.Repositories.Contract;
using SportsShop.Core.Services.Contract;
using SportsShop.Core.Specifications.Orders;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsShop.Service.Helpers
{
    public class PaymentHandle:IPaymentHandle
    {
        private readonly ILogger<PaymentHandle> _logger;
        private readonly IGenericRepository<Order> _orderRepo;

        public PaymentHandle(ILogger<PaymentHandle> logger, IGenericRepository<Order> orderRepo)
        {
            _logger = logger;
            _orderRepo = orderRepo;
        }

        public Event ConstructStripeEvent(HttpRequest Request,string json, string _whSecret)
        {
            try
            {
                return EventUtility.ConstructEvent(json, Request.Headers["Stripe-Signature"],
                    _whSecret);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to construct stripe event");
                throw new StripeException("Invalid signature");
            }
        }

        public async Task HandlePaymentIntentSucceeded(PaymentIntent intent)
        {
            if (intent.Status == "succeeded")
            {
                var spec = new OrderSpecification(intent.Id, true);

                var order = await _orderRepo.GetWithSpecAsync(spec)
                    ?? throw new Exception("Order not found");

                if ((long)order?.GetTotal() * 100 != intent.Amount)
                {
                    order.Status = OrderStatus.PaymentMismatch;
                }
                else
                {
                    order.Status = OrderStatus.PaymentReceived;
                }
                await _orderRepo.SaveChangesAsync();

                // TODO: SignalR

            }
        }

    }
}
