using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cryptocop.Software.API.Models.Exceptions
{
    public class OrderException : Exception
    {
        public OrderException() : base("") {}
        public OrderException(string message) : base(message) {}
        public OrderException(string message, Exception inner) : base(message, inner) {}
    }
}