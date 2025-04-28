using MediatR;
using SportsShop.Core.Dtos;
using SportsShop.Core.Dtos.ShoppingCart;
using SportsShop.Core.Repositories.Contract;
using SportsShop.Service.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsShop.Service.CQRS.ShoppingCarts.Queries
{
    public record GetShoppingCartQuery(string key):IRequest<ResultDto?>;

    public class GetShoppingCartQueryHandler : IRequestHandler<GetShoppingCartQuery, ResultDto?>
    {
        private readonly IShoppingCartRepository _shoppingCartRepository;

        public GetShoppingCartQueryHandler(IShoppingCartRepository shoppingCartRepository)
        {
            _shoppingCartRepository = shoppingCartRepository;
        }

        public async Task<ResultDto?> Handle(GetShoppingCartQuery request, CancellationToken cancellationToken)
        {
            var cart = await _shoppingCartRepository.GetCartAsync(request.key);
            if (cart != null)
            {
                return ResultDto.Sucess(cart.Mapone<ShoppingCartDto>());
            }
            return null;
        }
    }

}
