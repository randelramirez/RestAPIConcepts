using RestAPIConcepts.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestAPIConcepts.ViewModels
{
    [NameMustBeDifferentWithAddress(ErrorMessage = "Name and Address cannot be the same")]
    public class CreateSupplierGuidViewModel
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(200)]
        public string Address { get; set; }

        // PRODUCTS?
    }

    public class CreateSupplierGuidViewModelOld : IValidatableObject
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(200)]
        public string Address { get; set; }

        // PRODUCTS?

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Name == Address)
            {
                // we don't always need to return the class name,
                // but since this a class property validation, we used the actual class name instead
                yield return new ValidationResult("Thr provided description should be different from the title",
                    new[] {"CourseForCreationDto"});
            }
        }
    }
}
