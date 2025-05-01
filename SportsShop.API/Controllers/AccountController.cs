using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using SportsShop.API.Extensions;
using SportsShop.Core.Dtos.User;
using SportsShop.Core.Entities;
using SportsShop.Core.Services.Contract;
using SportsShop.Service.CQRS.User.Commands;
using System.Security.Claims;

namespace SportsShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(IMediator mediator, SignInManager<AppUser> signInManager)
        {
            _mediator = mediator;
            _signInManager = signInManager;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterDto registerDto)
        {
            var result= await _mediator.Send(new RegisterUserCommand(registerDto));

            if (!result.IsSuccess)
            {
                foreach (var error in result.Data)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
                return ValidationProblem();
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

        [Authorize]
        [HttpPost("logout")]
        public async Task<ActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            Response.Cookies.Delete("access_token");
            return NoContent();
        }

        
        [HttpGet("user-info")]
        public async Task<ActionResult> GetUserInfo()
        {
            if(User.Identity?.IsAuthenticated == false) return NoContent();

            var user = await _signInManager.UserManager.GetUserByEmailWithAddress(User);


            return Ok(new 
            {
                user.FirstName,
                user.LastName,
                user.Email,
                Address = user.Address?.ToDto()
            });
        }

        [HttpGet]
        public ActionResult GetAuthState()
        {
            return Ok(new { IsAuthenticated = User.Identity?.IsAuthenticated ?? false });
        }

        [Authorize]
        [HttpPost("address")]
        public async Task<ActionResult<Address>> CreateOrUpdateAddress(AddressDto addressDto)
        {
            var user = await _signInManager.UserManager.GetUserByEmailWithAddress(User);

            if (user.Address == null)
            {
                user.Address = addressDto.ToEntity();
            }
            else
            {
                user.Address.UpdateFromDto(addressDto);
            }

            var result = await _signInManager.UserManager.UpdateAsync(user);

            if (!result.Succeeded) return BadRequest("Problem updating user address");

            return Ok(user.Address.ToDto());

        }

    }
}
