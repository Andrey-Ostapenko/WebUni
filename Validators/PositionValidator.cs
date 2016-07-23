using FluentValidation;
using livemenu.Common.Entities.Entities;
using livemenu.Kernel.Positions;

namespace livemenu.Validators
{
    public class PositionValidator : AbstractValidator<Position>
    {
        public PositionValidator(IPositionService positionService)
        {
            RuleFor(x => x.ID).NotNull().WithMessage(ValidationMessages.Validation_FieldRequired);
            RuleFor(x => x.Name).NotEmpty().WithMessage(ValidationMessages.Validation_FieldRequired);
            RuleFor(x => x.Name).Must((u, name) => !positionService.IsExist(name)).WithMessage(ValidationMessages.Validation_SuchObjectAlreadyExists);
        }
    }
}