using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SportsShop.Core.Dtos.ShoppingCart;
using SportsShop.Core.Entities;
using SportsShop.Core.Repositories.Contract;
using SportsShop.Service.CQRS.DeliveryMethods.Queries;
using SportsShop.Service.CQRS.Payment.Commands;
using SportsShop.Service.Helpers;

namespace SportsShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PaymentsController(IMediator mediator)
        {
            _mediator = mediator;
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

    }
}
