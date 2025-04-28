using MediatR;
using SportsShop.Core.Dtos;
using SportsShop.Core.Dtos.ShoppingCart;
using SportsShop.Core.Entities;
using SportsShop.Core.Repositories.Contract;
using SportsShop.Service.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsShop.Service.CQRS.ShoppingCarts.Commands
{
    public record AddOrUpdateShoppingCartCommand(ShoppingCartDto ShoppingCartDto):IRequest<ResultDto?>;

    public class AddOrUpdateShoppingCartCommandHandler : IRequestHandler<AddOrUpdateShoppingCartCommand, ResultDto?>
    {
        private readonly IShoppingCartRepository _shoppingCartRepository;

        public AddOrUpdateShoppingCartCommandHandler(IShoppingCartRepository shoppingCartRepository)
        {
            _shoppingCartRepository = shoppingCartRepository;
        }

        public async Task<ResultDto?> Handle(AddOrUpdateShoppingCartCommand request, CancellationToken cancellationToken)
        {
            var shopCart = await _shoppingCartRepository.SetCartAsync(request.ShoppingCartDto.Mapone<ShoppingCart>());

            if (shopCart != null)
            {
                return ResultDto.Sucess(shopCart.Mapone<ShoppingCartDto>());
            }

            return null;
        }
    }

}
