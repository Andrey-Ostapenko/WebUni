using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using livemenu.Common;
using livemenu.Common.Entities.Entities;
using livemenu.Kernel.Account;
using livemenu.Kernel.Authentication;

namespace livemenu.Validators.Profile
{
    internal class AccountChangePasswordModelValidator : AbstractValidator<AccountChangePasswordModel>
    {
        public AccountChangePasswordModelValidator(IContainerFactory containerFactory, IAccountManager accountManager)
        {

            RuleFor(x => x.LastPassword)
                .NotEmpty().WithMessage(ValidationMessages.Validation_FieldRequired);
            RuleFor(x => x.LastPassword)
                .Must((u, password) => !string.IsNullOrEmpty(password) && containerFactory.Instantiate<IPasswordChanger>(accountManager.GetCurrentUser().PasswordMethod).CheckPassword(password)).WithMessage(ValidationMessages.Validation_PasswordNotCorrent);

            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage(ValidationMessages.Validation_FieldRequired)
                .Matches("^[a-zA-Z0-9_]+$").WithMessage(ValidationMessages.Validation_UserPasswordRegExp);

            RuleFor(x => x.NewPasswordConfirm).NotEmpty().WithMessage(ValidationMessages.Validation_FieldRequired)
                .Matches("^[a-zA-Z0-9_]+$").WithMessage(ValidationMessages.Validation_UserPasswordRegExp)
                .Must((u, password) => u.NewPasswordConfirm == u.NewPassword).WithMessage(ValidationMessages.Validation_PasswordAreNotEqual);

        }
    }
}
