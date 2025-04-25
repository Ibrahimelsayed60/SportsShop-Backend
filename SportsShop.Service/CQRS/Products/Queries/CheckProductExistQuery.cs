using MediatR;
using SportsShop.Core.Dtos;
using SportsShop.Core.Entities;
using SportsShop.Core.Repositories.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsShop.Service.CQRS.Products.Queries
{
    public record CheckProductExistQuery(int productId): IRequest<bool>;

    public class CheckProductExistQueryHandler : IRequestHandler<CheckProductExistQuery, bool>
    {
        private readonly IGenericRepository<Product> _productRepo;

        public CheckProductExistQueryHandler(IGenericRepository<Product> productRepo)
        {
            _productRepo = productRepo;
        }

        public Task<bool> Handle(CheckProductExistQuery request, CancellationToken cancellationToken)
        {
            bool isExist = _productRepo.Exists(request.productId);

            return Task.FromResult( isExist);
        }
    }

}
