using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SportsShop.Core.Dtos.User;
using SportsShop.Service.CQRS.User.Commands;

namespace SportsShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AccountController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterDto registerDto)
        {
            var result= await _mediator.Send(new RegisterUserCommand(registerDto));

            if (!result.Data.Succeeded)
            {
                return BadRequest(result.Data.Errors);
            }
            return Ok();
        }

    }
}
