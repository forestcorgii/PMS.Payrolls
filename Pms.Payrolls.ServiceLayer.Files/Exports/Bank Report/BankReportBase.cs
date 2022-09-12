using Pms.Payrolls.Domain;
using Pms.Payrolls.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Pms.Payrolls.Domain.Enums;

namespace Pms.Payrolls.ServiceLayer.Files.Exports.Bank_Report
{
    public class BankReportBase : IExportBankReportService
    {
        private Dictionary<BankChoices, IExportBankReportService> Exporters;

        public BankReportBase(string cutoffId, string payrollCode)
        {
            Cutoff cutoff = new(cutoffId);

            Exporters = new();
            Exporters.Add(BankChoices.CHK, new CHKExporter(cutoff, payrollCode));
            Exporters.Add(BankChoices.LBP, new LBPExporter(cutoff, payrollCode));
            Exporters.Add(BankChoices.CBC, new CBCExporter(cutoff, payrollCode));
            Exporters.Add(BankChoices.CBC1, new CBCExporter(cutoff, payrollCode));
            Exporters.Add(BankChoices.MPALO, new MBExporter(cutoff, payrollCode, "MPALO"));
            Exporters.Add(BankChoices.MTAC, new MBExporter(cutoff, payrollCode, "MTAC"));
        }

        public void StartExport(IEnumerable<Payroll> payrolls)
        {
            Dictionary<BankChoices, List<Payroll>> payrollsByBank = payrolls.GroupBy(p => p.EE.Bank).Select(pp => pp.ToList()).ToDictionary(pp => pp.First().EE.Bank);
            foreach (BankChoices bank in payrollsByBank.Keys)
                Exporters[bank].StartExport(payrollsByBank[bank]);
        }
    }
}
