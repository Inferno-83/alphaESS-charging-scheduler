using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace AlphaESS_API.Data
{
    [Serializable]
    public class Response
    {
        [JsonPropertyName("code")]
        [JsonInclude]
        public int Code { get; private set; }

        [JsonPropertyName("msg")]
        [JsonInclude]
        public string? Message { get; private set; }

        [JsonPropertyName("data")]
        [JsonInclude]
        public JsonNode? Data { get; private set; }

        public static readonly Response Invalid = new();
    }

    public enum ResponseCode
    {
        Success = 200,
    }
}
