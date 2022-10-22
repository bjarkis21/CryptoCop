using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Cryptocop.Software.API.Models.Attributes;

namespace Cryptocop.Software.API.Models.InputModels
{
    public class RegisterInputModel
    {
        [Required]
        [EmailAddress]
        public string Email {get; set;}
        [Required]
        [MinLength(3)]
        public string FullName {get; set;}
        [Required]
        [MinLength(8)]
        public string Password {get; set;}
        [PasswordMatching]
        [Required]
        public string PasswordConfirmation {get; set;}
        
    }
}