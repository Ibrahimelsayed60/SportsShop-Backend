using MediatR;
using SportsShop.Core.Dtos;
using SportsShop.Core.Entities.Order;
using SportsShop.Core.Repositories.Contract;
using SportsShop.Core.Specifications.Orders;
using SportsShop.Service.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsShop.Service.CQRS.Orders.Queries
{
    public record GetOrdersForUserByIdQuery(OrderSpecification spec) : IRequest<ResultDto>;

    public class GetOrdersForUserByIdQueryHandler : IRequestHandler<GetOrdersForUserByIdQuery, ResultDto>
    {
        private readonly IGenericRepository<Order> _orderRepo;

        public GetOrdersForUserByIdQueryHandler(IGenericRepository<Order> orderRepo)
        {
            _orderRepo = orderRepo;
        }

        public async Task<ResultDto> Handle(GetOrdersForUserByIdQuery request, CancellationToken cancellationToken)
        {
            var order = await _orderRepo.GetWithSpecAsync(request.spec);

            var orderDto = order?.ToDto();

            return ResultDto.Sucess(orderDto!);
        }
    }
}
