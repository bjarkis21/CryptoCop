using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cryptocop.Software.API.Models.Exceptions
{
    public class ModelFormatException : Exception
    {
        public ModelFormatException() : base("Model is not properly formatted.") {}
        public ModelFormatException(string message) : base(message) {}
        public ModelFormatException(string message, Exception inner) : base(message, inner) {}
    }
}