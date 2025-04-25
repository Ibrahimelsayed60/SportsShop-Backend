using MediatR;
using SportsShop.Core.Dtos;
using SportsShop.Core.Entities;
using SportsShop.Core.Repositories.Contract;
using SportsShop.Core.Specifications.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsShop.Service.CQRS.Products.Queries
{
    public record GetProductsCountQuery(ProductSpecParams ProductSpecParams) : IRequest<int>;

    public class GetProductsCountQueryHandler : IRequestHandler<GetProductsCountQuery, int>
    {
        private readonly IGenericRepository<Product> _productRepo;

        public GetProductsCountQueryHandler(IGenericRepository<Product> productRepo)
        {
            _productRepo = productRepo;
        }

        public async Task<int> Handle(GetProductsCountQuery request, CancellationToken cancellationToken)
        {
            var spec = new ProductCountSpecification(request.ProductSpecParams);

            var count = await _productRepo.GetCountAsync(spec);

            return count;

        }
    }

}
