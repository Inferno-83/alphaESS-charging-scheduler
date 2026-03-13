using AlphaESS_API;
using AlphaESS_API.Extensions;
using Microsoft.VisualBasic;
using System.Text.Json;

namespace AlphaESS_API.Test
{
    public class AlphaESSCredentials
    {
        public string System { get; set; }
        public string AppId { get; set; }
        public string AppSecret { get; set; }
    }

    [DoNotParallelize]
    [TestClass]
    [DeploymentItem("credentials.json")]
    public sealed class AlphaESSTest
    {
        [TestMethod]
        public void TestReadApi()
        {
            // TODO - manually create the credentials.json file with your system's settings
            string credentialsString = File.ReadAllText("credentials.json");

            var credentials = JsonSerializer.Deserialize<AlphaESSCredentials>(credentialsString);


            using var http = new HttpClient();

            var alphaESS = new HttpAlphaESS(credentials.AppId, credentials.AppSecret);

            var systems = alphaESS.GetSystems();

            Assert.IsNotNull(systems);

            Console.WriteLine($"System Ids: {string.Join(",", systems)}");

            Assert.AreNotEqual(0, systems.Count);
            var firstSystem = systems.First();

            Assert.AreEqual(credentials.System, firstSystem.SerialNumber);

            var systemSummary = alphaESS.GetSystemSummaryDataForToday(credentials.System);
            var loadSnapshot = alphaESS.GetSystemLoadSnapshot(credentials.System);
            var chargeConfigResponse = alphaESS.GetChargeConfig(credentials.System);
            var dischargeConfigResponse = alphaESS.GetDischargeConfig(credentials.System);

            Console.WriteLine($"Systems");
            Console.WriteLine("=======================");
            Console.WriteLine(systems.ToPrettyJson());

            Console.WriteLine($"System Summary");
            Console.WriteLine("=======================");
            Console.WriteLine(systemSummary.ToPrettyJson());

            Console.WriteLine($"Last Power Data");
            Console.WriteLine("=======================");
            Console.WriteLine(loadSnapshot.ToPrettyJson());

            Console.WriteLine($"Charge Config");
            Console.WriteLine("=======================");
            Console.WriteLine(chargeConfigResponse.ToPrettyJson());

            Console.WriteLine($"Discharge Config");
            Console.WriteLine("=======================");
            Console.WriteLine(dischargeConfigResponse.ToPrettyJson());

            var date = DateTime.Now.Subtract(TimeSpan.FromDays(1));

            var powerDataHistorical = alphaESS.GetPowerDataForDay(credentials.System, date);
            var energyDataHistorical = alphaESS.GetEnergyDataForDay(credentials.System, date);

            Console.WriteLine($"Power Data Historical - {date:yyyy-MM-dd}");
            Console.WriteLine("=======================");
            Console.WriteLine(powerDataHistorical.ToPrettyJson());

            Console.WriteLine($"Energy Data Historical - {date:yyyy-MM-dd}");
            Console.WriteLine("=======================");
            Console.WriteLine(energyDataHistorical.ToPrettyJson());
        }
    }
}
