using MediatR;
using SportsShop.Core.Dtos;
using SportsShop.Core.Entities;
using SportsShop.Core.Repositories.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsShop.Service.CQRS.Products.Commands
{
    public record DeleteProductCommand(int productId): IRequest<ResultDto>;

    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, ResultDto>
    {
        private readonly IGenericRepository<Product> _productRepo;

        public DeleteProductCommandHandler(IGenericRepository<Product> productRepo)
        {
            _productRepo = productRepo;
        }

        public async Task<ResultDto> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _productRepo.GetByIdAsync(request.productId);

            _productRepo.Delete(product);

            await _productRepo.SaveChangesAsync();

            return ResultDto.Sucess(true, "Product is Deleted");
        }
    }

}
