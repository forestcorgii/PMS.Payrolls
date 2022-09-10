using Xunit;
using Pms.Payrolls.ServiceLayer.Files.Exports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Pms.Payrolls.Persistence;
using Pms.Payrolls.Domain.Services;
using Pms.Payrolls.Tests;
using Pms.Payrolls.ServiceLayer.EfCore;
using Pms.Payrolls.Domain;
using static Pms.Payrolls.Domain.Enums;

namespace Pms.Payrolls.ServiceLayer.Files.Exports.Tests
{
    public class AlphalistVerifierExporterTests
    {
        private IDbContextFactory<PayrollDbContext> _factory;
        private IProvidePayrollService _payrollProvider;

        public AlphalistVerifierExporterTests()
        {
            _factory = new PayrollDbContextFactoryFixture();
            _payrollProvider = new PayrollProvider(_factory);
        }

        [Fact()]
        public void ShouldExportAlphaVerifier()
        {
            Company company = new() { RegisteredName = "TEST COMPANY", MinimumRate = 71.25 };
            int yearCovered = 2022;
            IEnumerable<Payroll> payrolls = _payrollProvider.GetPayrolls(yearCovered, company.CompanyId);
            var employeePayrolls = payrolls.GroupBy(py => py.EEId).Select(py => py.ToList()).ToList();

            AlphalistVerifierExporter exporter = new();
            exporter.StartExport(employeePayrolls,yearCovered,company);
        }
    }
}