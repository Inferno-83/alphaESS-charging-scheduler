using AlphaESS_API.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlphaESS_API
{
    public interface IAlphaESS
    {
        IReadOnlyList<Data.System>? GetSystems();

        SystemSummaryData? GetSystemSummaryDataForToday(string sysSn);

        SystemLoadSnapshot? GetSystemLoadSnapshot(string sysSn);

        IReadOnlyList<PowerData>? GetPowerDataForDay(string sysSn, DateTime date);

        EnergyData? GetEnergyDataForDay(string sysSn, DateTime date);

        ChargeConfig? GetChargeConfig(string sysSn);

        bool SetChargeConfig(string sysSn, ChargeConfig chargeConfig);

        DischargeConfig? GetDischargeConfig(string sysSn);

        bool SetDischargeConfig(string sysSn, DischargeConfig dischargeConfig);
    }
}
