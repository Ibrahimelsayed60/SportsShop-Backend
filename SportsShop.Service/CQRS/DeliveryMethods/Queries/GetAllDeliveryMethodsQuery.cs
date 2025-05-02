using MediatR;
using SportsShop.Core.Entities;
using SportsShop.Core.Repositories.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsShop.Service.CQRS.DeliveryMethods.Queries
{
    public record GetAllDeliveryMethodsQuery():IRequest<IReadOnlyList<DeliveryMethod>>;

    public class GetAllDeliveryMethodsQueryHandler : IRequestHandler<GetAllDeliveryMethodsQuery, IReadOnlyList<DeliveryMethod>>
    {
        private readonly IGenericRepository<DeliveryMethod> _deliveryRepo;

        public GetAllDeliveryMethodsQueryHandler(IGenericRepository<DeliveryMethod> deliveryRepo)
        {
            _deliveryRepo = deliveryRepo;
        }

        public async Task<IReadOnlyList<DeliveryMethod>> Handle(GetAllDeliveryMethodsQuery request, CancellationToken cancellationToken)
        {
            return (await _deliveryRepo.GetAllAsync()).ToList();
        }
    }

}
