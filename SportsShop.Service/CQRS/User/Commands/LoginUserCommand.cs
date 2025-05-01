using MediatR;
using Microsoft.AspNetCore.Identity;
using SportsShop.Core.Dtos;
using SportsShop.Core.Dtos.User;
using SportsShop.Core.Entities;
using SportsShop.Core.Services.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsShop.Service.CQRS.User.Commands
{
    public record LoginUserCommand(LoginDto loginDto):IRequest<ResultDto>;

    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, ResultDto>
    {
        private readonly IAuthService _authService;
        private readonly SignInManager<AppUser> _signInManager;

        public LoginUserCommandHandler(IAuthService authService, SignInManager<AppUser> signInManager)
        {
            _authService = authService;
            _signInManager = signInManager;
        }

        public async Task<ResultDto> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _signInManager.UserManager.FindByEmailAsync(request.loginDto.Email);

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.loginDto.Password, false);

            if (!result.Succeeded)
            {
                return ResultDto.Faliure("Can not Sign up with this Email");
            }

            var userReturned = new UserDto
            {
                FirstName = user.FirstName,
                LastName=user.LastName,
                Email = user.Email,
                Token = await _authService.CreateTokenAsync(user, _signInManager.UserManager)
            };

            return ResultDto.Sucess(userReturned);
        }
    }

}
