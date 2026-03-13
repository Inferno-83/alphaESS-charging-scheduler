using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AlphaESS_API.Extensions
{
    public static class JsonExtensions
    {
        public static string ToPrettyJson(this object? obj)
        {
            if (obj == null)
                return "{}";

            var options = new JsonSerializerOptions
            {
                WriteIndented = true // Enables pretty printing
            };

            return JsonSerializer.Serialize(obj, options);
        }
    }
}
