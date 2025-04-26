using FluentValidation;
using SportsShop.Service.CQRS.Products.Commands;

namespace SportsShop.API.Validators
{
    public class ProductCreateDtoValidators : AbstractValidator<ProductCreateDto>
    {

        public ProductCreateDtoValidators()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is Required!");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required!");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than 0.");

            RuleFor(x => x.PictureUrl)
                .NotEmpty().WithMessage("PictureUrl is required!");

            RuleFor(x => x.Type)
                .NotEmpty().WithMessage("Type is required!");

            RuleFor(x => x.Brand)
                .NotEmpty().WithMessage("Brand is required!");

            RuleFor(x => x.QuantityInStock)
                .GreaterThanOrEqualTo(1).WithMessage("Quantity in stock must be at least 1.");
        }

    }
}
