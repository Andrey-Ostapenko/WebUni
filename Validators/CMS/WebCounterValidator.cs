using FluentValidation;
using livemenu.Common.Entities.Entities;

namespace livemenu.Validators.CMS
{
    public class WebCounterValidator : AbstractValidator<WebCounter>
    {
        public WebCounterValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(ValidationMessages.Validation_FieldRequired);
        }
    }
}