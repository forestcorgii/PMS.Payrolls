using Xunit;
using Pms.Payrolls.ServiceLayer.EfCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Pms.Payrolls.Persistence;
using Pms.Payrolls.Domain.Services;
using Pms.Payrolls.Tests;

using static Pms.Payrolls.Tests.Seeder;
using Pms.Payrolls.Domain;
using static Pms.Payrolls.Domain.Enums;

namespace Pms.Payrolls.ServiceLayer.EfCore.Tests
{
    public class PayrollManagerTests
    {
        private IDbContextFactory<PayrollDbContext> _factory;
        private IProvidePayrollService _payrollProvider;
        private IManagePayrollService _payrollManager;

        public PayrollManagerTests()
        {
            _factory = new PayrollDbContextFactoryFixture();
            _payrollProvider = new PayrollProvider(_factory);
            _payrollManager = new PayrollManager(_factory);
        }

        [Fact()]
        public void ShouldSavePayrollSuccessfully()
        {
            Payroll expected = GenerateSeedPayroll("DYYJ", "2207-1",  "MANILAIDCSI0000", 19000, 20000, 17000, -1000, -1000, -1000);

            _payrollManager.SavePayroll(expected);

            Payroll actual = _payrollProvider.GetPayrolls(expected.CutoffId, expected.PayrollCode).Where(p => p.PayrollId == expected.PayrollId).FirstOrDefault();
            Assert.NotNull(actual);
        }
    }
}