using Xunit;
using Pms.Payrolls.ServiceLayer.Files.Exports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pms.Payrolls.ServiceLayer.EfCore;
using Pms.Payrolls.Tests;
using Pms.Payrolls.Domain.Services;
using Microsoft.EntityFrameworkCore;
using Pms.Payrolls.Persistence;
using Pms.Payrolls.Domain;
using static Pms.Payrolls.Domain.Enums;
using Pms.Payrolls.Domain.SupportTypes;

namespace Pms.Payrolls.Files.Exports.Tests
{
    public class ThirteenthMonthExportTests
    {
        private IDbContextFactory<PayrollDbContext> _factory;
        private IProvidePayrollService _payrollProvider;

        public ThirteenthMonthExportTests()
        {
            _factory = new PayrollDbContextFactoryFixture();
            _payrollProvider = new PayrollProvider(_factory);
        }

        [Fact()]
        public void ShouldExportThirteenthMonth()
        {
            int yearCovered = 2021;
            IEnumerable<Payroll> payrolls = _payrollProvider.GetPayrolls(yearCovered, BankType.LBP);
            List<string> eeIds = payrolls.ExtractEEIds();

            List<ThirteenthMonth> thirteenthMonths = new();
            foreach (string eeId in eeIds)
            {
                IEnumerable<Payroll> eePayrolls = payrolls.Where(p => p.EEId == eeId);

                double totalRegPay = eePayrolls.Sum(p => p.AdjustedRegPay());
                double computed13Month = totalRegPay / 12;

                thirteenthMonths.Add(new ThirteenthMonth()
                {
                    EEId = eeId,
                    TotalRegPay = totalRegPay,
                    Amount = computed13Month
                });
            }
            Assert.NotEmpty(thirteenthMonths);

            ThirteenthMonthExport exporter = new();
            exporter.StartExport(thirteenthMonths, yearCovered, BankType.LBP);
        }
    }
}