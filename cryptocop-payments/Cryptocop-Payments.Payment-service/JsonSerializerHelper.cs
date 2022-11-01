using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Cryptocop_Payments.Payment_service
{
    public static class JsonSerializerHelper
    {
        private static JsonSerializerOptions _options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        public static T? DeserializeWithCamelCasing<T>(string message) =>
            JsonSerializer.Deserialize<T>(message, _options);

        public static string SerializeWithCamelCasing<T>(T obj) =>
            JsonSerializer.Serialize(obj, _options);
    }
}