using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SportsShop.Core.Dtos;
using SportsShop.Core.Dtos.ShoppingCart;
using SportsShop.Service.CQRS.ShoppingCarts.Commands;
using SportsShop.Service.CQRS.ShoppingCarts.Queries;

namespace SportsShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CartController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ResultDto> GetCartById(string id)
        {
            var cart = await _mediator.Send(new GetShoppingCartQuery(id));
            return cart?? ResultDto.Sucess(cart);
        }

        [HttpPost]
        public async Task<ActionResult<ResultDto>> UpdateCart(ShoppingCartDto shoppingCartDto)
        {
            var cart = await _mediator.Send(new AddOrUpdateShoppingCartCommand(shoppingCartDto));
            
            if (cart == null) return BadRequest("Problem with cart");

            return Ok(cart);
        }

        [HttpDelete]
        public async Task<ActionResult<ResultDto>> DeleteCart(string id)
        {
            var result = await _mediator.Send(new DeleteShoppingCartCommand(id));

            if (!result) return BadRequest("Problem deleting cart");

            return Ok(ResultDto.Sucess(true));
        }

    }
}
