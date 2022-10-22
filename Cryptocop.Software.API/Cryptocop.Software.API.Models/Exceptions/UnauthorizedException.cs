using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cryptocop.Software.API.Models.Exceptions
{
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException() : base("Unauthorized") {}
        public UnauthorizedException(string message) : base(message) {}
        public UnauthorizedException(string message, Exception inner) : base(message, inner) {}
    }
}