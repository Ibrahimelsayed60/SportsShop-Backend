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
    public record GetBrandsOfProductsQuery():IRequest<ResultDto>;

    public class GetBrandsOfProductsQueryHandler : IRequestHandler<GetBrandsOfProductsQuery, ResultDto>
    {
        private readonly IGenericRepository<Product> _productRepo;

        public GetBrandsOfProductsQueryHandler(IGenericRepository<Product> productRepo)
        {
            _productRepo = productRepo;
        }

        public async Task<ResultDto> Handle(GetBrandsOfProductsQuery request, CancellationToken cancellationToken)
        {
            var spec = new BrandListSpecification();

            var brands = await _productRepo.ListOptionalAsync(spec);

            return ResultDto.Sucess(brands);
        }
    }

}
