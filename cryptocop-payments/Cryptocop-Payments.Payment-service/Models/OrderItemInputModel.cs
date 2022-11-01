using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cryptocop_Payments.Payment_service.Models
{
    public class OrderItemInputModel
    {
        public int Id {get; set;}
        public string ProductIdentifier {get; set;} = "";
        public double Quantity {get; set;}
        public double UnitPrice {get; set;}
        public double TotalPrice {get; set;}
    }
}