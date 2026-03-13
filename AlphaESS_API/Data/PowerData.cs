using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AlphaESS_API.Data
{
    public class PowerData
    {
        [JsonPropertyName("cobat")]
        [JsonInclude]
        public double Battery { get; private set; } = 0;

        [JsonPropertyName("feedIn")]
        [JsonInclude]
        public double FeedIn { get; private set; } = 0;

        [JsonPropertyName("gridCharge")]
        [JsonInclude]
        public double Grid { get; private set; } = 0;

        [JsonPropertyName("load")]
        [JsonInclude]
        public double Load { get; private set; } = 0;

        [JsonPropertyName("pChargingPile")]
        [JsonInclude]
        public double Charging { get; private set; } = 0;

        [JsonPropertyName("ppv")]
        [JsonInclude]
        public double Generation { get; private set; } = 0;

        [JsonPropertyName("sysSn")]
        [JsonInclude]
        public string SerialNumber { get; private set; } = string.Empty;

        [JsonPropertyName("uploadTime")]
        [JsonInclude]
        public string UploadTime { get; private set; } = string.Empty;
    }
}
