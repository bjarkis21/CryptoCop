using System.ComponentModel.DataAnnotations;

namespace Cryptocop.Software.API.Models.InputModels
{
    public class PaymentCardInputModel
    {
        [Required]
        [MinLength(3)]
        public string CardholderName { get; set; }
        [Required]
        [CreditCard]
        public string CardNumber { get; set; }
        [Required]
        [Range(1, 12)]
        public int? Month { get; set; }
        [Required]
        [Range(0,99)]
        public int? Year { get; set; }
    }
}