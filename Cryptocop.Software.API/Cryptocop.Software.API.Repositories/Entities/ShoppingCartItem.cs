using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cryptocop.Software.API.Repositories.Entities
{
    public class ShoppingCartItem
    {
        public int Id {get; set;}
        public int ShoppingCartId {get; set;}
        public string ProductIdentifier {get; set;}
        public double Quantity {get; set;}
        public double UnitPrice {get; set;}

        public ShoppingCart ShoppingCart {get; set;}
    }
}