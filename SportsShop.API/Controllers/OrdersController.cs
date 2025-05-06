using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SportsShop.API.ControllerParameter;
using SportsShop.API.Extensions;
using SportsShop.Core.Dtos;
using SportsShop.Core.Dtos.Orders;
using SportsShop.Core.Entities.Order;
using SportsShop.Core.Specifications.Orders;
using SportsShop.Service.CQRS.Orders.Commands;
using SportsShop.Service.CQRS.Orders.Queries;

namespace SportsShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<ResultDto>> CreateOrder(CreateOrderDto createOrderDto)
        {
            var email = User.GetEmail();
            return await _mediator.Send(new CreateOrderCommand(email, createOrderDto));
        }

        [HttpGet]
        public async Task<ActionResult<ResultDto>> GetOrdersByUser()
        {
            var spec = new OrderSpecification(User.GetEmail());

            var orders = await _mediator.Send(new GetOrdersForUserQuery(spec));

            return orders;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ResultDto>> GetOrderById(int id)
        {
            var spec = new OrderSpecification(User.GetEmail(), id);

            var order = await _mediator.Send(new GetOrdersForUserByIdQuery(spec));

            if(order.Data == null)
                return NotFound();

            return Ok(order);
        }


    }
}
