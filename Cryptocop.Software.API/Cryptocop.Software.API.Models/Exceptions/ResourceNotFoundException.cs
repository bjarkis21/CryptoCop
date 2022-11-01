using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cryptocop.Software.API.Models.Exceptions
{
    public class ResourceNotFoundException : Exception
    {
        public ResourceNotFoundException() : base("The Resource was not found") {}
        public ResourceNotFoundException(string message) : base(message) {}
        public ResourceNotFoundException(string message, Exception inner) : base(message, inner) {}
    }
}