using MediatR;
using SportsShop.Core.Dtos;
using SportsShop.Core.Dtos.Products;
using SportsShop.Core.Entities;
using SportsShop.Core.Repositories.Contract;
using SportsShop.Service.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsShop.Service.CQRS.Products.Queries
{
    public record GetProductByIdQuery(int productId):IRequest<ResultDto>;

    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ResultDto>
    {
        private readonly IGenericRepository<Product> _productRepo;

        public GetProductByIdQueryHandler(IGenericRepository<Product> productRepo)
        {
            _productRepo = productRepo;
        }

        public async Task<ResultDto> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var product = await _productRepo.GetByIdAsync(request.productId);

            if (product is null)
            {
                return ResultDto.Faliure("The project isn't found!");
            }

            return ResultDto.Sucess(product.Mapone<ProductDto>());
        }
    }

}
