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

    public class Pagination<T>(int pageIndex, int pageSize, int count, IEnumerable<T> data)
    {
        public int PageIndex { get; set; } = pageIndex;
        public int PageSize { get; set; } = pageSize;
        public int Count { get; set; } = count;
        public IEnumerable<T> Data { get; set; } = data;
    }

    public class GtAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, ResultDto>
    {
        private readonly IGenericRepository<Product> _productRepo;
        private readonly IMediator _mediator;

        public GtAllProductsQueryHandler(IGenericRepository<Product> productRepo, IMediator mediator)
        {
            _productRepo = productRepo;
            _mediator = mediator;
        }
        public async Task<ResultDto> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            var spec = new ProductSpecification(request.ProductSpecParams);

            var products = await _productRepo.GetAllWithSpecAsync(spec);

            var productsCount = await _mediator.Send(new GetProductsCountQuery(request.ProductSpecParams));

            var productsDtos = products.Map<ProductDto>();

            var paginatedData = new Pagination<ProductDto>(request.ProductSpecParams.PageIndex, request.ProductSpecParams.PageSize, productsCount, productsDtos);

            return ResultDto.Sucess(paginatedData);
        }
    }

}
