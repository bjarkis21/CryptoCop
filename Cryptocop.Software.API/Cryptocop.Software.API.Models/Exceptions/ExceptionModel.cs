using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Cryptocop.Software.API.Models.Exceptions
{
    public class ExceptionModel
    {
        public int StatusCode {get; set;}
        public string Message {get; set;}
        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}