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
        private BankChoices _bankType;
        public BankReportBase(BankChoices bankType)
        {
            _bankType = bankType;
        }

        public void StartExport(IEnumerable<Payroll> payrolls, string cutoffId, string payrollCode)
        {
            switch (_bankType)
            {
                case BankChoices.LBP:
                    new LBPExporter().StartExport(payrolls, cutoffId, payrollCode);
                    break;
            }
        }
    }
}
