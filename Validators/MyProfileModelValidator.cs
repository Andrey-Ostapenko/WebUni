using FluentValidation;
using livemenu.Kernel.Account;

namespace livemenu.Validators
{
    public class MyProfileModelValidator : AbstractValidator<MyProfileModel>
    {
        public MyProfileModelValidator()
        {
            RuleFor(x => x.ID).NotNull().WithMessage(ValidationMessages.Validation_FieldRequired);
            RuleFor(x => x.FirstName).NotEmpty().WithMessage(ValidationMessages.Validation_FieldRequired);
            RuleFor(x => x.Password).NotEmpty().When(x => x.IsPasswordChanged).WithMessage(ValidationMessages.Validation_FieldRequired);
            RuleFor(x => x.Password).Matches("^[a-zA-Z0-9_]+$").When(x => x.IsPasswordChanged).WithMessage(ValidationMessages.Validation_UserPasswordRegExp);
        }
    }
}