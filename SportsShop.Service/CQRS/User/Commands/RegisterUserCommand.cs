using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
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
    public record RegisterUserCommand(RegisterDto registerDto):IRequest<ResultDto>;

    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, ResultDto>
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IAuthService _authService;

        public RegisterUserCommandHandler(SignInManager<AppUser> signInManager, IAuthService authService)
        {
            _signInManager = signInManager;
            _authService = authService;
        }

        public async Task<ResultDto> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var user = new AppUser
            {
                FirstName = request.registerDto.FirstName,
                LastName = request.registerDto.LastName,
                Email = request.registerDto.Email,
                UserName = request.registerDto.Email.Split('@')[0]
            };

            var result = await _signInManager.UserManager.CreateAsync(user, request.registerDto.Password);

            

            if (!result.Succeeded)
            {
                return ResultDto.Faliure("Can not Sign up with this Email");
            }

            var userDataReturned = new UserDto
            {
                DisplayName = request.registerDto.FirstName + " " + request.registerDto.LastName,
                Email = request.registerDto.Email,
                Token = await _authService.CreateTokenAsync(user, _signInManager.UserManager)
            };

            return ResultDto.Sucess(userDataReturned);

        }
    }

}
