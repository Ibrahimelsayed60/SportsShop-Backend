using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using SportsShop.Core.Dtos;
using SportsShop.Core.Dtos.User;
using SportsShop.Core.Entities;
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

        public RegisterUserCommandHandler(SignInManager<AppUser> signInManager)
        {
            _signInManager = signInManager;
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

            return ResultDto.Sucess(result);


        }
    }

}
