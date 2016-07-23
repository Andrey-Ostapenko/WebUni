using FluentValidation;
using livemenu.Kernel.Catalog;

namespace livemenu.Validators
{
    public class CatalogItemCardModelValidator : AbstractValidator<CatalogItemCardModel>
    {
        public CatalogItemCardModelValidator(ICatalogItemService catalogItemService)
        {
            //RuleFor(x => x.ID).NotNull().WithMessage(ValidationMessages.Validation_FieldRequired);
            RuleFor(x => x.Name).NotEmpty().WithMessage(ValidationMessages.Validation_FieldRequired);
            RuleFor(x => x.Code).NotEmpty().WithMessage(ValidationMessages.Validation_FieldRequired);
            RuleFor(x => x.Code).Must((u, code) =>
             u.IsLink || (!u.IsNew && !catalogItemService.IsExistAnother(code, u.ID)) || (u.IsNew && !catalogItemService.IsExist(code))).WithMessage(ValidationMessages.Validation_SuchObjectAlreadyExists);


            RuleFor(x => x.Code).Matches("^[a-zA-Z0-9-_]+$").WithMessage(ValidationMessages.Validation_CatalogItemRegExp);
        }
    }
}