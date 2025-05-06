using Hangfire;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SportsShop.API.Extensions;
using SportsShop.API.SignalR;
using SportsShop.Core.Dtos.Orders;
using SportsShop.Core.Dtos.ShoppingCart;
using SportsShop.Core.Entities;
using SportsShop.Core.Entities.Order;
using SportsShop.Core.Repositories.Contract;
using SportsShop.Core.Services.Contract;
using SportsShop.Core.Specifications.Orders;
using SportsShop.Service.CQRS.DeliveryMethods.Queries;
using SportsShop.Service.CQRS.Orders.Queries;
using SportsShop.Service.CQRS.Payment.Commands;
using SportsShop.Service.Helpers;
using Stripe;

namespace SportsShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<PaymentsController> _logger;
        private readonly IGenericRepository<Order> _orderRepo;
        private readonly IPaymentHandle _paymentHandle;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly IConfiguration _config;
        private readonly string _whSecret = "";

        public PaymentsController(IMediator mediator, 
            ILogger<PaymentsController> logger, 
            IGenericRepository<Order> orderRepo,
            IPaymentHandle paymentHandle,
            IHubContext<NotificationHub> hubContext,
            IConfiguration config)
        {
            _mediator = mediator;
            _logger = logger;
            _orderRepo = orderRepo;
            _paymentHandle = paymentHandle;
            _hubContext = hubContext;
            _config = config;
            _whSecret = _config["StripeSettings:WhSecret"]!;
        }

        [Authorize]
        [HttpPost("{cartId}")]
        public async Task<ActionResult<ShoppingCart>> CreateOrUpdatePaymentIntent(string cartId)
        {
            var cart = await _mediator.Send(new CreateOrUpdatePaymentIntentCommand(cartId));

            if (cart == null) return BadRequest("Problem with your cart");

            return Ok(((ShoppingCartDto)cart.Data).Mapone<ShoppingCart>());
        }

        [HttpGet("delivery-methods")]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethods()
        {
            return Ok( await _mediator.Send( new GetAllDeliveryMethodsQuery()));
        }

        [HttpPost("webhook")]
        public async Task<ActionResult> StripeWebhook()
        {
            var json = await new StreamReader(Request.Body).ReadToEndAsync();

            try
            {
                var stripeEvent = _paymentHandle.ConstructStripeEvent(Request,json, _whSecret);

                if (stripeEvent.Data.Object is not PaymentIntent intent)
                {
                    return BadRequest("Invalid event data");
                }

                BackgroundJob.Enqueue(() => _paymentHandle.HandlePaymentIntentSucceeded(intent));

                var spec = new OrderSpecification(intent.Id, true);

                var order = await _orderRepo.GetWithSpecAsync(spec)
                    ?? throw new Exception("Order not found");

                var connectionId = NotificationHub.GetConnectionIdByEmail(User.GetEmail());

                if (!string.IsNullOrEmpty(connectionId))
                {
                    await _hubContext.Clients.Client(connectionId)
                        .SendAsync("OrderCompleteNotification", order.ToDto());
                }

                //await HandlePaymentIntentSucceeded(intent);

                return Ok();
            }
            catch (StripeException ex)
            {
                _logger.LogError(ex, "Stripe webhook error");
                return StatusCode(StatusCodes.Status500InternalServerError, "Webhook error");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred");
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred");
            }


        }

        //public async Task HandlePaymentIntentSucceeded(PaymentIntent intent)
        //{
        //    if(intent.Status == "succeeded")
        //    {
        //        var spec = new OrderSpecification(intent.Id, true);

        //        var order = await _orderRepo.GetWithSpecAsync(spec)
        //            ?? throw new Exception("Order not found");

        //        if((long) order?.GetTotal() * 100 != intent.Amount)
        //        {
        //            order.Status = OrderStatus.PaymentMismatch;
        //        }
        //        else
        //        {
        //            order.Status = OrderStatus.PaymentReceived;
        //        }
        //        await _orderRepo.SaveChangesAsync();

        //        // TODO: SignalR

        //    }
        //}

        //private Event ConstructStripeEvent(string json)
        //{
        //    try
        //    {
        //        return EventUtility.ConstructEvent(json, Request.Headers["Stripe-Signature"],
        //            _whSecret);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Failed to construct stripe event");
        //        throw new StripeException("Invalid signature");
        //    }
        //}

    }
}
