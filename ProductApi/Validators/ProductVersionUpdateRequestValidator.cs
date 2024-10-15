using FluentValidation;
using ProductApi.Models.Requests;

namespace ProductApi.Validators
{
    public class ProductVersionUpdateRequestValidator : AbstractValidator<ProductVersionUpdateRequest>
    {
        public ProductVersionUpdateRequestValidator()
        {
            RuleFor(x => x.ProductID)
                .NotEmpty().WithMessage("ProductID is required.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(255).WithMessage("Name cannot exceed 255 characters.");

            RuleFor(x => x.Width)
                .GreaterThan(0).WithMessage("Width must be a positive value.");

            RuleFor(x => x.Height)
                .GreaterThan(0).WithMessage("Height must be a positive value.");

            RuleFor(x => x.Length)
                .GreaterThan(0).WithMessage("Length must be a positive value.");
        }
    }
}
