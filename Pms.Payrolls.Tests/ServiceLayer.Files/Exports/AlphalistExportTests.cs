﻿using Xunit;
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
using Pms.Payrolls.Domain.SupportTypes;
using static Pms.Payrolls.Domain.Enums;

namespace Pms.Payrolls.ServiceLayer.Files.Exports.Tests
{
    public class AlphalistExportTests
    {
        private IDbContextFactory<PayrollDbContext> _factory;
        private IProvidePayrollService _payrollProvider;

        public AlphalistExportTests()
        {
            _factory = new PayrollDbContextFactoryFixture();
            _payrollProvider = new PayrollProvider(_factory);
        }
        [Fact()]
        public void ShouldExportAlphalist()
        {
            int yearCovered = 2022;
            IEnumerable<Payroll> payrolls = _payrollProvider.GetPayrolls(yearCovered,BankChoices.LBP);
            var employeePayrolls = payrolls.GroupBy(py=>py.EEId).Select(py=>py.ToList()).ToList();

            List<AlphalistDetail> alphalists = new();
            foreach (var employeePayroll in employeePayrolls)
                alphalists.Add(new AutomatedAlphalistDetail(employeePayroll,71.25).CreateAlphalistDetail());

            Assert.NotEmpty(alphalists);

            AlphalistExport exporter = new();
            exporter.StartExport(alphalists, yearCovered,new Company() { RegisteredName="TEST COMPANY"});
        }
    }
}