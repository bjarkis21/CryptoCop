using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cryptocop.Software.API.Models.Exceptions
{
    public class ProductException: Exception
    {
        public ProductException() : base("Wrong product") {}
        public ProductException(string message) : base(message) {}
        public ProductException(string message, Exception inner) : base(message, inner) {}
    }
}