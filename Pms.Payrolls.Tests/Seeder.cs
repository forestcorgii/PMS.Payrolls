using Pms.Payrolls.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Pms.Payrolls.Domain.Enums;

namespace Pms.Payrolls.Tests
{
    static class Seeder
    {
        public static Payroll GenerateSeedPayroll(string eeId, string cutoffId, BankType bank, double grossPay, double regPay, double netPay, double adjust1Total, double adjust2Total, double governmentTotal)
        {
            Payroll payroll = new()
            {
                EEId = eeId,
                CutoffId = cutoffId,
                PayrollCode = "P1A",
                BankCategory = "CCARD",
                Bank = bank,
                GrossPay = grossPay,
                RegPay = regPay,
                NetPay = netPay,
                Adjust1Total = adjust1Total,
                Adjust2Total = adjust2Total,
                GovernmentTotal = governmentTotal,
            };
            payroll.PayrollId = Payroll.GenerateId(payroll);
            return payroll;
        }
    }
}
