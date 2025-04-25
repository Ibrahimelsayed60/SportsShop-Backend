using MediatR;
using SportsShop.Core.Dtos;
using SportsShop.Core.Dtos.Products;
using SportsShop.Core.Entities;
using SportsShop.Core.Repositories.Contract;
using SportsShop.Core.Specifications.Products;
using SportsShop.Service.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsShop.Service.CQRS.Products.Queries
{
    public record GetAllProductsQuery(ProductSpecParams ProductSpecParams): IRequest<ResultDto>;

    public class GtAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, ResultDto>
    {
        private readonly IGenericRepository<Product> _productRepo;

        public GtAllProductsQueryHandler(IGenericRepository<Product> productRepo)
        {
            _productRepo = productRepo;
        }
        public async Task<ResultDto> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            var spec = new ProductSpecification(request.ProductSpecParams);

            var products = await _productRepo.GetAllWithSpecAsync(spec);

            var productsDtos = products.Map<ProductDto>();

            return ResultDto.Sucess(productsDtos);
        }
    }

}
