using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AlphaESS_API.Data
{
    public class EnergyData
    {
        [JsonPropertyName("eCharge")]
        [JsonInclude]
        public double ChargedFromBattery { get; private set; } = 0;

        [JsonPropertyName("eChargingPile")]
        [JsonInclude]
        public double ChargingPilesConsumption { get; private set; } = 0;

        [JsonPropertyName("eGridCharge")]
        [JsonInclude]
        public double GridCharge { get; private set; } = 0;

        [JsonPropertyName("eGridDischarge")]
        [JsonInclude]
        public double GridDischarge { get; private set; } = 0;

        [JsonPropertyName("eInput")]
        [JsonInclude]
        public double GridConsumption { get; private set; } = 0;

        [JsonPropertyName("eOutput")]
        [JsonInclude]
        public double FeedIn { get; private set; } = 0;

        [JsonPropertyName("epv")]
        [JsonInclude]
        public double Generation { get; private set; } = 0;

        [JsonPropertyName("sysSn")]
        [JsonInclude]
        public string SerialNumber { get; private set; } = string.Empty;

        [JsonPropertyName("theDate")]
        [JsonInclude]
        public string Date { get; private set; } = string.Empty;
    }
}
