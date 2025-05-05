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
    public record GetOrdersForUserQuery(OrderSpecification spec):IRequest<ResultDto>;

    public class GetOrdersForUserQueryHandler : IRequestHandler<GetOrdersForUserQuery, ResultDto>
    {
        private readonly IGenericRepository<Order> _orderRepo;

        public GetOrdersForUserQueryHandler(IGenericRepository<Order> orderRepo)
        {
            _orderRepo = orderRepo;
        }

        public async Task<ResultDto> Handle(GetOrdersForUserQuery request, CancellationToken cancellationToken)
        {
            var orders = await _orderRepo.GetAllWithSpecAsync(request.spec);

            var ordersToReturn = orders.Select(o => o.ToDto()).ToList();

            return ResultDto.Sucess(ordersToReturn);
        }
    }

}
