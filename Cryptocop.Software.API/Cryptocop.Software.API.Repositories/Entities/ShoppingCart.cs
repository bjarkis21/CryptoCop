using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cryptocop.Software.API.Repositories.Entities
{
    public class ShoppingCart
    {
        public int Id {get; set;}
        public int UserId {get; set;}

        public User User {get; set;}
        public ICollection<ShoppingCartItem> ShoppingCartItems {get; set;}
    }
}