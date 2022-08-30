using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pms.Payrolls.Domain;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Pms.Payrolls.Persistence;
using Pms.Payrolls.Domain.Services;
using Pms.Payrolls.Tests;
using Pms.Payrolls.ServiceLayer.EfCore;
using Pms.Payrolls.ServiceLayer.Files.Exports;
using static Pms.Payrolls.Domain.Enums;
using Pms.Payrolls.ServiceLayer.Files.Exports.Bank_Report;

namespace Pms.Payrolls.Files.Exports.Tests
{
    public class LBPExportTests
    {
        private IDbContextFactory<PayrollDbContext> _factory;
        private IProvidePayrollService _payrollProvider;

        public LBPExportTests()
        {
            _factory = new PayrollDbContextFactoryFixture();
            _payrollProvider = new PayrollProvider(_factory);
        }

        [Fact()]
        public void ShouldExportPayroll()
        {
            string cutoffId = "2208-2";
            string payrollCode = "P1A";
            BankChoices bankType = BankChoices.LBP;
            IEnumerable<Payroll> payrolls = _payrollProvider.GetPayrolls(cutoffId, payrollCode, bankType);

            BankReportBase exporter = new(bankType);
            exporter.StartExport(payrolls.ToArray(), cutoffId, payrollCode);
        }
    }
}