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
            string cutoffId = "2208-1";
            IEnumerable<Payroll> payrolls = _payrollProvider.GetPayrolls(cutoffId, BankType.LBP);
            LBPExport export = new();
            export.StartExport(payrolls.ToArray(), cutoffId, "IDCSI");
        }
    }
}