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
    public class PayRegisterPDImportTests
    {

        [Fact()]
        public void ShouldImportPayroll()
        {
            IImportPayrollService payregImporter = new PayrollRegisterImportBase(Enums.ImportProcessChoices.PD);
            string payregFilePath = $@"{ AppDomain.CurrentDomain.BaseDirectory}\TESTDATA\PayRegisterImportTests\P7A201912A.xls";
            List<Payroll> actualPayrolls = payregImporter.StartImport(payregFilePath).ToList();

            Assert.NotNull(actualPayrolls);
            Assert.NotEmpty(actualPayrolls);
            Assert.True(actualPayrolls[0].RegHours > 0);
        }

        [Fact()]
        public void ShouldImportOldPayroll()
        {
            IImportPayrollService payregImporter = new PayrollRegisterImportBase(Enums.ImportProcessChoices.PD);
            string payregFilePath = $@"{ AppDomain.CurrentDomain.BaseDirectory}\TESTDATA\PayRegisterImportTests\P1A202201B.xls";
            List<Payroll> actualPayrolls = payregImporter.StartImport(payregFilePath).ToList();

            Assert.NotNull(actualPayrolls);
            Assert.NotEmpty(actualPayrolls);
            Assert.True(actualPayrolls[0].RegHours > 0);
        }
    }
}