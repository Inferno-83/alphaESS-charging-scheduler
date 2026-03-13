using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AlphaESS_API.Data
{
    public class SystemLoadSnapshot
    {
        [JsonPropertyName("ppv")]
        [JsonInclude]
        public double? TotalPower { get; private set; } = 0;

        [JsonPropertyName("ppvDetail")]
        [JsonInclude]
        public PvDetail? PvDetail { get; private set; } = new PvDetail();

        [JsonPropertyName("pload")]
        [JsonInclude]
        public double? Load { get; private set; } = 0;

        [JsonPropertyName("soc")]
        [JsonInclude]
        public double? Soc { get; private set; } = 0;

        [JsonPropertyName("pgrid")]
        [JsonInclude]
        public double? Grid { get; private set; } = 0;

        [JsonPropertyName("pgridDetail")]
        [JsonInclude]
        public GridDetail? GridDetail { get; private set; } = new GridDetail();

        [JsonPropertyName("pbat")]
        [JsonInclude]
        public double? Battery { get; private set; } = 0;

        [JsonPropertyName("prealL1")]
        [JsonInclude]
        public double? RealL1 { get; private set; } = 0;

        [JsonPropertyName("prealL2")]
        [JsonInclude]
        public double? RealL2 { get; private set; } = 0;

        [JsonPropertyName("prealL3")]
        [JsonInclude]
        public double? RealL3 { get; private set; } = 0;

        [JsonPropertyName("pev")]
        [JsonInclude]
        public double? ChargingPile { get; private set; } = 0;

        [JsonPropertyName("pevDetail")]
        [JsonInclude]
        public EvDetail? EvDetail { get; private set; } = new EvDetail();
    }

    public class PvDetail
    {
        [JsonInclude]
        public double? Ppv1 { get; private set; }

        [JsonInclude]
        public double? Ppv2 { get; private set; }

        [JsonInclude]
        public double? Ppv3 { get; private set; }

        [JsonInclude]
        public double? Ppv4 { get; private set; }

        [JsonPropertyName("pmeterDc")]
        [JsonInclude]
        public double? PowerMeterDc { get; private set; }
    }

    public class GridDetail
    {
        [JsonPropertyName("pmeterL1")]
        [JsonInclude]
        public double? PowerMeterL1 { get; private set; }

        [JsonPropertyName("pmeterL2")]
        [JsonInclude]
        public double? PowerMeterL2 { get; private set; }

        [JsonPropertyName("pmeterL3")]
        [JsonInclude]
        public double? PowerMeterL3 { get; private set; }
    }

    public class EvDetail
    {
        [JsonInclude]
        public double? Ev1Power { get; private set; }

        [JsonInclude]
        public double? Ev2Power { get; private set; }

        [JsonInclude]
        public double? Ev3Power { get; private set; }

        [JsonInclude]
        public double? Ev4Power { get; private set; }
    }
}
