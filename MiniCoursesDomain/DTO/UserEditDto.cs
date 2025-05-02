using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniCoursesDomain.DTO
{
    public class UserEditDto
    {
        public string? Id { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Last Name is required.")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Role is required.")]
        public string? Role { get; set; }

        [RequiredIfStudent(ErrorMessage = "Indeks is required for students.")]
        public string? Indeks { get; set; }
    }



    public class RequiredIfStudent : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var model = (UserEditDto)validationContext.ObjectInstance;

            if (model.Role == "Student" && string.IsNullOrWhiteSpace(value?.ToString()))
            {
                return new ValidationResult("Indeks is required for Students.");
            }

            return ValidationResult.Success;
        }
    }
}
