using System.Text.Json.Serialization;

namespace AlphaESS_API.Data
{
    public class SystemSummaryData
    {
        [JsonPropertyName("epvtoday")]
        [JsonInclude]
        public double GenerationToday { get; private set; } = 0;

        [JsonPropertyName("epftotal")]
        [JsonInclude]
        public double GenerationTotal { get; private set; } = 0;

        [JsonPropertyName("eload")]
        [JsonInclude]
        public double LoadToday { get; private set; } = 0;

        [JsonPropertyName("eoutput")]
        [JsonInclude]
        public double FeedInToday { get; private set; } = 0;

        [JsonPropertyName("einput")]
        [JsonInclude]
        public double ConsumedToday { get; private set; } = 0;

        [JsonPropertyName("echarge")]
        [JsonInclude]
        public double ChargedToday { get; private set; } = 0;

        [JsonPropertyName("edischarge")]
        [JsonInclude]
        public double DischargedToday { get; private set; } = 0;

        [JsonPropertyName("todayIncome")]
        [JsonInclude]
        public double IncomeToday { get; private set; } = 0;

        [JsonPropertyName("totalIncome")]
        [JsonInclude]
        public double IncomeTotal { get; private set; } = 0;

        [JsonPropertyName("eselfConsumption")]
        [JsonInclude]
        public double SelfConsumption { get; private set; } = 0;

        [JsonPropertyName("eselfSufficiency")]
        [JsonInclude]
        public double SelfSufficiency { get; private set; } = 0;

        [JsonPropertyName("treeNum")]
        [JsonInclude]
        public double TreesPlanted { get; private set; } = 0;

        [JsonPropertyName("carbonNum")]
        [JsonInclude]
        public double CarbonReduction { get; private set; } = 0;

        [JsonPropertyName("moneyType")]
        [JsonInclude]
        public string Currency { get; private set; } = string.Empty;
    }
}
