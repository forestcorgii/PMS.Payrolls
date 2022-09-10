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
    public class BankReportExportTests
    {
        private IDbContextFactory<PayrollDbContext> _factory;
        private IProvidePayrollService _payrollProvider;

        public BankReportExportTests()
        {
            _factory = new PayrollDbContextFactoryFixture();
            _payrollProvider = new PayrollProvider(_factory);
        }

        [Fact()]
        public void ShouldExportInMBFormat()
        {
            string cutoffId = "2209-1";
            string payrollCode = "P4A";
            IEnumerable<Payroll> payrolls = _payrollProvider.GetPayrolls(cutoffId, payrollCode);

            BankReportBase exporter = new(cutoffId, payrollCode);
            exporter.StartExport(payrolls.ToArray());
        }


        [Fact()]
        public void ShouldExportInLbpCbcChkFormat()
        {
            string cutoffId = "2208-1";
            string payrollCode = "P1A";
            IEnumerable<Payroll> payrolls = _payrollProvider.GetPayrolls(cutoffId, payrollCode);

            BankReportBase exporter = new(cutoffId, payrollCode);
            exporter.StartExport(payrolls.ToArray());
        }


    }
}