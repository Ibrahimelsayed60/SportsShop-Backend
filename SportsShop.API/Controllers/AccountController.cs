using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using SportsShop.Core.Dtos.User;
using SportsShop.Core.Services.Contract;
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

            if (!result.IsSuccess)
            {
                return BadRequest();
            }
            return Ok(result.Data);
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginDto loginDto)
        {
            var result= await _mediator.Send(new  LoginUserCommand(loginDto));
            if (!result.IsSuccess)
            {
                return BadRequest();
            }

            Response.Cookies.Append("access_token", result.Data.Token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddHours(1)
            });

            return Ok(result.Data);
        }

    }
}
