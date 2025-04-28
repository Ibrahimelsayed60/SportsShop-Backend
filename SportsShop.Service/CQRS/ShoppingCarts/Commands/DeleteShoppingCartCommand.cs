using MediatR;
using SportsShop.Core.Dtos;
using SportsShop.Core.Repositories.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsShop.Service.CQRS.ShoppingCarts.Commands
{
    public record DeleteShoppingCartCommand(string cartId) :IRequest<bool>;

    public class DeleteShoppongCartCommandHandler : IRequestHandler<DeleteShoppingCartCommand, bool>
    {
        private readonly IShoppingCartRepository _cartRepository;

        public DeleteShoppongCartCommandHandler(IShoppingCartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public async Task<bool> Handle(DeleteShoppingCartCommand request, CancellationToken cancellationToken)
        {
            return await _cartRepository.DeleteCartAsync(request.cartId);
        }
    }

}
