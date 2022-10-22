using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Cryptocop.Software.API.Models.InputModels;

namespace Cryptocop.Software.API.Models.Attributes
{
    public class PasswordMatchingAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            //return base.IsValid(value, validationContext);
            var registerInput = (RegisterInputModel) validationContext.ObjectInstance;

            if (registerInput.Password != registerInput.PasswordConfirmation)
            {
                return new ValidationResult("Password and PasswordConfirmation do not match");
            }
            return ValidationResult.Success;
        }
    }
}