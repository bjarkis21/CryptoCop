using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cryptocop.Software.API.Models.InputModels
{
    public class UpdateShoppingCartItemInputModel
    {
        [Required]
        [Range(0.01, double.MaxValue)]
        public double? Quantity {get; set;}
    }
}