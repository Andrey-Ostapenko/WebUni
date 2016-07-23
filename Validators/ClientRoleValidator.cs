using FluentValidation;
using livemenu.Common.Entities.Entities;

namespace livemenu.Validators
{
    public class ClientRoleValidator : AbstractValidator<ClientRole>
    {
        public ClientRoleValidator()
        {
            RuleFor(x => x.ID).NotNull().WithMessage(ValidationMessages.Validation_FieldRequired);
            RuleFor(x => x.Group).NotNull().WithMessage(ValidationMessages.Validation_FieldRequired);
            RuleFor(x => x.Name).NotEmpty().WithMessage(ValidationMessages.Validation_FieldRequired);
        }

    }

    public class ClientRoleStatusValidator : AbstractValidator<ClientRoleStatus>
    {
        public ClientRoleStatusValidator()
        {
            RuleFor(x => x.ID).NotNull().WithMessage(ValidationMessages.Validation_FieldRequired);
            RuleFor(x => x.Group).NotNull().WithMessage(ValidationMessages.Validation_FieldRequired);
            RuleFor(x => x.Name).NotEmpty().WithMessage(ValidationMessages.Validation_FieldRequired);
        }
    }
}
    