using MediatR;
using SportsShop.Core.Dtos;
using SportsShop.Core.Entities;
using SportsShop.Core.Repositories.Contract;
using SportsShop.Core.Specifications.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace SportsShop.Service.CQRS.Products.Queries
{
    public record GetTypesOfProductsQuery():IRequest<ResultDto>;

    public class GetTypesOfProductsQueryHandler : IRequestHandler<GetTypesOfProductsQuery, ResultDto>
    {
        private readonly IGenericRepository<Product> _productRepo;

        public GetTypesOfProductsQueryHandler(IGenericRepository<Product> productRepo)
        {
            _productRepo = productRepo;
        }

        public async Task<ResultDto> Handle(GetTypesOfProductsQuery request, CancellationToken cancellationToken)
        {
            var spec = new TypeListSpecification();

            var types = await _productRepo.ListOptionalAsync(spec);

            return ResultDto.Sucess(types);
        }
    }

}
