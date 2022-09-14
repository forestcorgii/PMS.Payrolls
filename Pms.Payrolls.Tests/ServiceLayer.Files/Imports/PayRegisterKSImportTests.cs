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
    public class PayRegisterKSImportTests
    {

        [Fact()]
        public void ShouldImportPayroll()
        {
            IImportPayrollService payregImporter = new PayrollRegisterImportBase(Enums.ImportProcessChoices.KS);
            string payregFilePath = $@"{AppDomain.CurrentDomain.BaseDirectory}\TESTDATA\PayRegisterImportTests\K13202208B.XLS";
            List<Payroll> actualPayrolls = payregImporter.StartImport(payregFilePath).ToList();

            Assert.NotNull(actualPayrolls);
            Assert.NotEmpty(actualPayrolls);
            Assert.True(actualPayrolls[0].RegHours > 0);
        }

        [Fact()]
        public void ShouldImportK12Payroll()
        {
            IImportPayrollService payregImporter = new PayrollRegisterImportBase(Enums.ImportProcessChoices.KS);
            string payregFilePath = $@"{ AppDomain.CurrentDomain.BaseDirectory}\TESTDATA\PayRegisterImportTests\K12202208B.XLS";
            List<Payroll> actualPayrolls = payregImporter.StartImport(payregFilePath).ToList();

            Assert.NotNull(actualPayrolls);
            Assert.NotEmpty(actualPayrolls);
            Assert.True(actualPayrolls[0].RegHours > 0);
        }


        [Fact()]
        public void ShouldImportK9APayroll()
        {
            IImportPayrollService payregImporter = new PayrollRegisterImportBase(Enums.ImportProcessChoices.KS);
            string payregFilePath = $@"{ AppDomain.CurrentDomain.BaseDirectory}\TESTDATA\PayRegisterImportTests\K9APAYREG.xls";
            List<Payroll> actualPayrolls = payregImporter.StartImport(payregFilePath).ToList();

            Assert.NotNull(actualPayrolls);
            Assert.NotEmpty(actualPayrolls);
            Assert.True(actualPayrolls[0].RegHours > 0);
        }

        
    }
}