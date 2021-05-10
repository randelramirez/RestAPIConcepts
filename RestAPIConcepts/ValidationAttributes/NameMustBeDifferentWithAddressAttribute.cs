using RestAPIConcepts.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace RestAPIConcepts.ValidationAttributes
{
    public class NameMustBeDifferentWithAddressAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value,
          ValidationContext validationContext)
        {
            // if this was for a property and not a class level attribute then the value is actual value of the property where the attribute is applied to
            var supplier = (validationContext.ObjectInstance as CreateSupplierGuidViewModel);

            if (supplier.Name == supplier.Address)
            {
                return new ValidationResult(ErrorMessage,
                    new[] { nameof(CreateSupplierGuidViewModel) });
            }

            return ValidationResult.Success;
        }
    }
}
