using System.Text.Json.Serialization;

namespace AlphaESS_API.Data
{
    public class DischargeConfig : IConfig
    {
        [JsonPropertyName("batHighCap")]
        [JsonInclude]
        public double DischargingCutoffPercentage { get; private set; } = 0;

        [JsonPropertyName("ctrDis")]
        [JsonInclude]
        public int Enabled { get; private set; } = 0;

        [JsonPropertyName("timeDisf1")]
        [JsonInclude]
        public string DischargePeriod1StartTime { get; private set; } = string.Empty;

        [JsonPropertyName("timeDise1")]
        [JsonInclude]
        public string DischargePeriod1EndTime { get; private set; } = string.Empty;

        [JsonPropertyName("timeDisf2")]
        [JsonInclude]
        public string DischargePeriod2StartTime { get; private set; } = string.Empty;

        [JsonPropertyName("timeDise2")]
        [JsonInclude]
        public string DischargePeriod2EndTime { get; private set; } = string.Empty;

        public Dictionary<string, string> ToArgs()
        {
            throw new NotImplementedException();
        }
    }
}
