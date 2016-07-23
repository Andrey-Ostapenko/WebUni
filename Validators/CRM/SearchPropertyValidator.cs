using FluentValidation;
using livemenu.Common.Entities.Entities;

namespace livemenu.Validators.CRM
{
    public class SearchPropertyValidator : AbstractValidator<SearchProperty>
    {
        public SearchPropertyValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(ValidationMessages.Validation_FieldRequired);
        }
    }
}