using FluentValidation;
using SportsShop.Core.Dtos.Orders;

namespace SportsShop.API.Validators
{
    public class CreateOrderDtoValidators: AbstractValidator<CreateOrderDto>
    {
        public CreateOrderDtoValidators()
        {
            RuleFor(x => x.CartId)
                .NotEmpty().WithMessage("CartId is Required!");

            RuleFor(x => x.DeliveryMethodId)
                .NotEmpty().WithMessage("DeliveryMethodId is Required!");

            RuleFor(x => x.ShippingAddress)
                .NotEmpty().WithMessage("ShippingAddress is Required!");

            RuleFor(x => x.PaymentSummary)
                .NotEmpty().WithMessage("PaymentSummary is Required!");

        }
    }
}
