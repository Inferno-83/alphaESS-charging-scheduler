using System.Text.Json.Serialization;

namespace AlphaESS_API.Data
{
    public class ChargeConfig : IConfig
    {
        [JsonPropertyName("batHighCap")]
        [JsonInclude]
        public double StopChargingPercentage { get; private set; } = 0;

        [JsonPropertyName("gridCharge")]
        [JsonInclude]
        public int Enabled { get; private set; } = 0;

        [JsonPropertyName("timeChaf1")]
        [JsonInclude]
        public string ChargePeriod1StartTime { get; private set; } = string.Empty;

        [JsonPropertyName("timeChae1")]
        [JsonInclude]
        public string ChargePeriod1EndTime { get; private set; } = string.Empty;

        [JsonPropertyName("timeChaf2")]
        [JsonInclude]
        public string ChargePeriod2StartTime { get; private set; } = string.Empty;

        [JsonPropertyName("timeChae2")]
        [JsonInclude]
        public string ChargePeriod2EndTime { get; private set; } = string.Empty;

        public Dictionary<string, string> ToArgs()
        {
            throw new NotImplementedException();
        }
    }
}
