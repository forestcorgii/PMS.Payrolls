using Xunit;
using Pms.Payrolls.Files;
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
using Pms.Payrolls.Files.Exports;
using Pms.Payrolls.ServiceLayer.Files;
using System.IO;

namespace Pms.Payrolls.Files.Tests
{
    public class PayRegisterImportTests
    {
        private IDbContextFactory<PayrollDbContext> _factory;
        private IProvidePayrollService _payrollProvider;
        private IManagePayrollService _payrollManager;

        public PayRegisterImportTests()
        {
            _factory = new PayrollDbContextFactoryFixture();
            _payrollProvider = new PayrollProvider(_factory);
            _payrollManager = new PayrollManager(_factory);
        }

        [Fact()]
        public void ShouldImportPayroll()
        {
            IImportPayrollService payregImporter = new PayRegisterImport();
            string payregDirectory = $@"{ AppDomain.CurrentDomain.BaseDirectory}\TESTDATA\P7A_2020";
            string[] payRegFiles = Directory.GetFiles(payregDirectory);
            foreach (string payregFilePath in payRegFiles)
            {
                List<Payroll> actualPayrolls = payregImporter.StartImport(payregFilePath).ToList();
                foreach (Payroll payroll in actualPayrolls)
                {
                    payroll.PayrollCode = "P7A";
                    payroll.Bank = Enums.BankType.LBP;
                    _payrollManager.SavePayroll(payroll);
                }

                Assert.NotNull(actualPayrolls);
                Assert.NotEmpty(actualPayrolls);
                Assert.True(actualPayrolls[0].RegHours > 0);
            }
        }
    }
}