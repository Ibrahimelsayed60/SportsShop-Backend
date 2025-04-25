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
    public record UpdateProductCommand(int productId, ProductCreateDto productCreateDto): IRequest<ResultDto>;

    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, ResultDto>
    {
        private readonly IGenericRepository<Product> _productRepo;

        public UpdateProductCommandHandler(IGenericRepository<Product> productRepo)
        {
            _productRepo = productRepo;
        }

        public async Task<ResultDto> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _productRepo.GetByIdAsync(request.productId);

            if(product is null)
            {
                return ResultDto.Faliure("Product is not found");
            }

            product = request.productCreateDto.Mapone<Product>();
            product.Id = request.productId;

            _productRepo.Update(product);

            await _productRepo.SaveChangesAsync();

            return ResultDto.Sucess(product, "Project updated successfully!");

        }
    }


}
