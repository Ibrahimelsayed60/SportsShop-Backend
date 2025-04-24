using MediatR;

namespace SportsShop.API.ControllerParameter
{
    public class ControllerParameters
    {
        public IMediator Mediator { get; set; }
        public ControllerParameters(IMediator mediator)
        {
            Mediator = mediator;
        }

    }
}
