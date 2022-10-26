using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cryptocop.Software.API.Models.Exceptions
{
    public class CartItemException : Exception
    {
        public CartItemException() : base("Wrong product") {}
        public CartItemException(string message) : base(message) {}
        public CartItemException(string message, Exception inner) : base(message, inner) {}
    }
}