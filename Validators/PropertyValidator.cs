using FluentValidation;
using livemenu.Common.Entities.Entities;
using livemenu.Kernel.Account;

namespace livemenu.Validators
{
    public class PropertyValidator : AbstractValidator<Property>
    {
        public PropertyValidator()
        {
            RuleFor(x => x.ID).NotNull().WithMessage(ValidationMessages.Validation_FieldRequired);
            RuleFor(x => x.Group).NotEmpty().WithMessage(ValidationMessages.Validation_FieldRequired);
            RuleFor(x => x.Name).NotEmpty().WithMessage(ValidationMessages.Validation_FieldRequired);
        }
    }
}