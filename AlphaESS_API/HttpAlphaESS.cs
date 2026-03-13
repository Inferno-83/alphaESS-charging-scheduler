using AlphaESS_API.Data;
using System;
using System.Collections.Specialized;
using System.Net.Http;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Web;
using static System.Net.WebRequestMethods;

namespace AlphaESS_API
{
    public class HttpAlphaESS : IAlphaESS, IDisposable
    {
        private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            NumberHandling = JsonNumberHandling.AllowReadingFromString
        };

        // Australia endpoint
        public static readonly string DefaultUrl = "https://openapi.alphaess.com/api";

        private HttpClient? _httpClient;
        private readonly string _appId;
        private readonly string _appSecret;
        private readonly string _baseUrl;

        public HttpAlphaESS(string appId, string appSecret, string? baseUrl = null)
        {
            _httpClient = new HttpClient();
            _appId = appId;
            _appSecret = appSecret;
            _baseUrl = baseUrl ?? DefaultUrl;
        }

        public IReadOnlyList<Data.System>? GetSystems()
        {
            var response = Get("getEssList", []);

            var systems = response.Data?.Deserialize<List<Data.System>>();

            return systems;
        }

        public SystemSummaryData? GetSystemSummaryDataForToday(string sysSn)
        {
            var response = GetCommandWithSn("getSumDataForCustomer", sysSn);

            var data = ProcessResponseData<SystemSummaryData>(response);

            return data;
        }

        public SystemLoadSnapshot? GetSystemLoadSnapshot(string sysSn)
        {
            var response = GetCommandWithSn("getLastPowerData", sysSn);

            var data = ProcessResponseData<SystemLoadSnapshot>(response);

            return data;
        }

        public IReadOnlyList<PowerData>? GetPowerDataForDay(string sysSn, DateTime date)
        {
            var response = GetCommandWithSnAndDate("getOneDayPowerBySn", sysSn, date);

            var data = ProcessResponseData<List<PowerData>>(response);

            return data;
        }

        public EnergyData? GetEnergyDataForDay(string sysSn, DateTime date)
        {
            var response = GetCommandWithSnAndDate("getOneDateEnergyBySn", sysSn, date);

            var data = ProcessResponseData<EnergyData>(response);

            return data;
        }

        public ChargeConfig? GetChargeConfig(string sysSn)
        {
            var response = GetCommandWithSn("getChargeConfigInfo", sysSn);

            var config = ProcessResponseData<ChargeConfig>(response);

            return config;
        }

        public bool SetChargeConfig(string sysSn, ChargeConfig chargeConfig)
        {
            var response = PostCommandWithSnForConfig("setDisChargeConfigInfo", sysSn, chargeConfig);

            return response.Code == (int)ResponseCode.Success;
        }

        public DischargeConfig? GetDischargeConfig(string sysSn)
        {
            var response = GetCommandWithSn("getDisChargeConfigInfo", sysSn);

            var config = ProcessResponseData<DischargeConfig>(response);

            return config;
        }

        public bool SetDischargeConfig(string sysSn, DischargeConfig dischargeConfig)
        {
            var response = PostCommandWithSnForConfig("setDisChargeConfigInfo", sysSn, dischargeConfig);

            return response.Code == (int) ResponseCode.Success;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);

            _httpClient?.Dispose();
            _httpClient = null;
        }

        private static T? ProcessResponseData<T>(Response? response)
        {
            if (response == Response.Invalid || response?.Data == null)
                return default;

            var config = JsonSerializer.Deserialize<T>(response.Data, JsonOptions);

            return config;
        }

        private Response GetCommandWithSn(string urlExt, string sysSn)
        {
            var getParams = new Dictionary<string, string>
                {
                    { "sysSn", sysSn }
                };

            var getResponse = Get(urlExt, getParams);

            return getResponse;

        }

        private Response GetCommandWithSnAndDate(string urlExt, string sysSn, DateTime date)
        {
            var getParams = new Dictionary<string, string>
                {
                    { "sysSn", sysSn },
                    { "queryDate", date.ToString("yyyy-MM-dd") }
                };

            var getResponse = Get(urlExt, getParams);

            return getResponse;
        }

        private Response Get(string urlExt, Dictionary<string, string> args)
        {
            return GetAsync(urlExt, args).GetAwaiter().GetResult();
        }

        private async Task<Response> GetAsync(string urlExt, Dictionary<string, string> args)
        {
            return await SendAsync(_httpClient, HttpMethod.Get, _baseUrl, urlExt, _appId, _appSecret, args);
        }
        
        private Response PostCommandWithSnForConfig(string urlExt, string sysSn, IConfig config)
        {
            var postParams = config.ToArgs();
            postParams["sysSn"] = sysSn;

            var getResponse = Post(urlExt, postParams);

            return getResponse;

        }

        private Response PostCommandWithSn(string urlExt, string sysSn, Dictionary<string, string> args)
        {
            var postParams = args.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            postParams["sysSn"] = sysSn;

            var getResponse = Post(urlExt, postParams);

            return getResponse;

        }

        private Response Post(string urlExt, Dictionary<string, string> args)
        {
            return PostAsync(urlExt, args).GetAwaiter().GetResult();
        }

        private async Task<Response> PostAsync(string urlExt, Dictionary<string, string> args)
        {
            return await SendAsync(_httpClient, HttpMethod.Post, _baseUrl, urlExt, _appId, _appSecret, args);
        }

        private static async Task<Response> SendAsync(
            HttpClient? httpClient,
            HttpMethod httpMethod,
            string baseUrl,
            string urlExt,
            string appId,
            string appSecret,
            Dictionary<string, string> args)
        {
            if (httpClient == null)
                return Response.Invalid;

            var uri = BuildUri($"{baseUrl}/{urlExt}", args);

            var httpRequest = new HttpRequestMessage(httpMethod, uri);

            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();

            var sign = CreateSignature(appId, appSecret, timestamp);

            httpRequest.Headers.Add("appId", appId);
            httpRequest.Headers.Add("timestamp", timestamp);
            httpRequest.Headers.Add("sign", sign);

            var httpResponse = await httpClient.SendAsync(httpRequest);
            httpResponse.EnsureSuccessStatusCode();

            var content = await httpResponse.Content.ReadAsStringAsync();

            var response = JsonSerializer.Deserialize<Response>(content, JsonOptions);

            return response ?? Response.Invalid;
        }

        private static Uri BuildUri(string uri, Dictionary<string, string> args)
        {
            var uriBuilder = new UriBuilder(uri);

            var query = HttpUtility.ParseQueryString(uriBuilder.Query);

            foreach (var arg in args)
            {
                query[arg.Key] = arg.Value;
            }

            uriBuilder.Query = query.ToString();

            return uriBuilder.Uri;
        }

        internal static string CreateSignature(
            string appId,
            string appSecret,
            string timestamp)
        {
            var sign = $"{appId}{appSecret}{timestamp}";

            return ComputeSHA512Hash(sign);
        }

        internal static string ComputeSHA512Hash(string input)
        {
            using (SHA512 sha512 = SHA512.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = sha512.ComputeHash(inputBytes);

                // Convert byte array to hex string
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }
    }
}
