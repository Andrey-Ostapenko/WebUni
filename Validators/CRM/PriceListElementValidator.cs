using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation;
using livemenu.Common.Entities.Entities;
using livemenu.Kernel.Catalog;
using livemenu.Kernel.CRM.PriceList;

namespace livemenu.Validators.CRM
{
    public class PriceListElementValidator : AbstractValidator<PriceListElement>
    {
        public PriceListElementValidator(IPriceListElementService priceListElementService)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(ValidationMessages.Validation_FieldRequired);
            RuleFor(x => x.Articul).NotEmpty().WithMessage(ValidationMessages.Validation_FieldRequired);
            //RuleFor(x => x.Group).NotEmpty().WithMessage(ValidationMessages.Validation_FieldRequired);
            
            //RuleFor(x => x.Articul).Must((u, code) =>
            //(!u.IsNew && !priceListElementService.IsExistAnother(code, u.ID)) || (u.IsNew && !priceListElementService.IsExist(code))).WithMessage(ValidationMessages.Validation_SuchObjectAlreadyExists);
        }
    }
}