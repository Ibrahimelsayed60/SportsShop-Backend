using MediatR;
using SportsShop.Core.Dtos;
using SportsShop.Core.Entities;
using SportsShop.Core.Repositories.Contract;
using SportsShop.Service.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsShop.Service.CQRS.Products.Commands
{
    public record AddProductCommand(ProductCreateDto ProductCreateDto):IRequest<ResultDto>;

    public record ProductCreateDto
    {
        public  string Name { get; set; }

        public  string Description { get; set; }

        public decimal Price { get; set; }

        public  string PictureUrl { get; set; }

        public  string Type { get; set; }

        public  string Brand { get; set; }

        public int QuantityInStock { get; set; }
    }

    public class AddProductCommandHandler : IRequestHandler<AddProductCommand, ResultDto>
    {
        private readonly IGenericRepository<Product> _productRepo;

        public AddProductCommandHandler(IGenericRepository<Product> productRepo)
        {
            _productRepo = productRepo;
        }

        public async Task<ResultDto> Handle(AddProductCommand request, CancellationToken cancellationToken)
        {
            var product = request.ProductCreateDto.Mapone<Product>();

            product = await _productRepo.AddAsync(product);

            await _productRepo.SaveChangesAsync();

            return ResultDto.Sucess(product);
        }
    }

}
