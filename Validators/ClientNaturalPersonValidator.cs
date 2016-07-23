using FluentValidation;
using livemenu.Common.Entities.Entities;

namespace livemenu.Validators
{
    public class ClientNaturalPersonValidator : AbstractValidator<ClientNaturalPerson>
    {
        public ClientNaturalPersonValidator()
        {
            RuleFor(x => x.Password).Matches("^[a-zA-Z0-9_]+$").When(x => x.IsPasswordChanged).WithMessage(ValidationMessages.Validation_UserPasswordRegExp);
        }
    }

    public class ClientLegalPersonalityValidator : AbstractValidator<ClientLegalPersonality>
    {
        public ClientLegalPersonalityValidator()
        {
            RuleFor(x => x.Password).Matches("^[a-zA-Z0-9_]+$").When(x => x.IsPasswordChanged).WithMessage(ValidationMessages.Validation_UserPasswordRegExp);
        }
    }

    public class ClientIndividualProprietorValidator : AbstractValidator<ClientIndividualProprietor>
    {
        public ClientIndividualProprietorValidator()
        {
            RuleFor(x => x.Password).Matches("^[a-zA-Z0-9_]+$").When(x => x.IsPasswordChanged).WithMessage(ValidationMessages.Validation_UserPasswordRegExp);
        }
    }
}