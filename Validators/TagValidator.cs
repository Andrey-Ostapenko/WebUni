using FluentValidation;
using livemenu.Common.Entities.Entities;
using livemenu.Kernel.Account;

namespace livemenu.Validators
{
    public class TagValidator : AbstractValidator<Tag>
    {
        public TagValidator()
        {
            RuleFor(x => x.ID).NotNull().WithMessage(ValidationMessages.Validation_FieldRequired);
            RuleFor(x => x.GroupID).NotEmpty().WithMessage(ValidationMessages.Validation_FieldRequired);
            RuleFor(x => x.Name).NotEmpty().WithMessage(ValidationMessages.Validation_FieldRequired);
        }
    }
}