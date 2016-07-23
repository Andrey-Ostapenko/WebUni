using FluentValidation;
using livemenu.Common.Entities.Entities;
using livemenu.Kernel.Users;

namespace livemenu.Validators
{
    public class UserValidator: AbstractValidator<User>
    {
        public UserValidator(IUserService userService)
        {
            RuleFor(x => x.ID).NotNull().WithMessage(ValidationMessages.Validation_FieldRequired);
            RuleFor(x => x.FirstName).NotEmpty().WithMessage(ValidationMessages.Validation_FieldRequired);
            RuleFor(x => x.NewLogin).NotEmpty().WithMessage(ValidationMessages.Validation_FieldRequired);
            RuleFor(x => x.NewLogin).Matches("^[a-zA-Z0-9_]+$").WithMessage(ValidationMessages.Validation_UserLoginRegExp);
            RuleFor(x => x.NewLogin).Must((u, login) =>
            {
                return (u.NewLogin == u.Login || !userService.IsExist(login));

            }).WithMessage(ValidationMessages.Validation_SuchObjectAlreadyExists);

            RuleFor(x => x.Password).NotEmpty().When(x => x.IsNew).WithMessage(ValidationMessages.Validation_FieldRequired);
            RuleFor(x => x.Password).Matches("^[a-zA-Z0-9_]+$").When(x => !x.IsNew && x.IsPasswordChanged).WithMessage(ValidationMessages.Validation_UserPasswordRegExp);

            RuleFor(x => x.NullablePositionID).NotNull().WithMessage(ValidationMessages.Validation_FieldRequired);

            RuleFor(x => x.Email).EmailAddress().When(x => !string.IsNullOrEmpty(x.Email)).WithMessage(ValidationMessages.Validation_EmailFormat);
        }
    }
}