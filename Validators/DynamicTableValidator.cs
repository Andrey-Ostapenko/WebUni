using FluentValidation;
using livemenu.Common.Entities.Entities;
using livemenu.Kernel.BWP.DynamicTables;
using Microsoft.Practices.ServiceLocation;
using System.Linq;

namespace livemenu.Validators
{
    public class DynamicTableValidator : AbstractValidator<DynamicTable>
    {
        public DynamicTableValidator()
        {
            RuleFor(x => x.ID).NotNull().WithMessage(ValidationMessages.Validation_FieldRequired);
            RuleFor(x => x.DisplayName).NotEmpty().WithMessage(ValidationMessages.Validation_FieldRequired);
            RuleFor(x => x.Description).NotEmpty().WithMessage(ValidationMessages.Validation_FieldRequired);
            RuleFor(x => x.Code).NotEmpty().WithMessage(ValidationMessages.Validation_FieldRequired);
            //RuleFor(x => x.Code).Must(UniqueDynamicTableCode).WithMessage(ValidationMessages.Validation_FieldMustBeUnique);
        }

        //private bool UniqueDynamicTableCode(string code)
        //{
        //    var service = ServiceLocator.Current.GetInstance<IDynamicTableService>();
        //    return !service.RawDataQueryable.Any(t => t.Code == code);
        //}
    }

    public class DynamicColumnValidator : AbstractValidator<DynamicColumn>
    {
        public DynamicColumnValidator()
        {
            RuleFor(x => x.ID).NotNull().WithMessage(ValidationMessages.Validation_FieldRequired);
            RuleFor(x => x.DisplayName).NotEmpty().WithMessage(ValidationMessages.Validation_FieldRequired);
            RuleFor(x => x.DynamicTypeID).NotNull().WithMessage(ValidationMessages.Validation_FieldRequired);
        }
    }

    public class DynamicColumnCodeValidator : AbstractValidator<DynamicColumnCode>
    {
        public DynamicColumnCodeValidator()
        {
            RuleFor(x => x.ID).NotNull().WithMessage(ValidationMessages.Validation_FieldRequired);
            RuleFor(x => x.DynamicColumnID).NotNull().WithMessage(ValidationMessages.Validation_FieldRequired);
            RuleFor(x => x.Code).NotNull().WithMessage(ValidationMessages.Validation_FieldRequired);
        }
    }
}
    