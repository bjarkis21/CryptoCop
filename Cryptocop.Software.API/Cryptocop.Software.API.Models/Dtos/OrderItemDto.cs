using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cryptocop.Software.API.Models.Dtos
{
    public class OrderItemDto
    {
        public int Id {get; set;}
        public string ProductIdentifier {get; set;}
        public double Quantity {get; set;}
        public double UnitPrice {get; set;}
        public double TotalPrice {get; set;}
    }
}