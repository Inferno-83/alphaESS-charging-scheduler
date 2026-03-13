using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AlphaESS_API.Data
{
    [Serializable]
    public class System
    {
        [JsonPropertyName("sysSn")]
        [JsonInclude]
        public string SerialNumber { get; private set; } = string.Empty;

        [JsonPropertyName("mbat")]
        [JsonInclude]
        public string BatteryModel { get; private set; } = string.Empty;

        [JsonPropertyName("minv")]
        [JsonInclude]
        public string InverterModel { get; private set; } = string.Empty;

        [JsonPropertyName("cobat")]
        [JsonInclude]
        public double BatteryCapacity { get; private set; } = 0;
    }
}
