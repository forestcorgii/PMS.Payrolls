﻿using Xunit;
using Pms.Payrolls.ServiceLayer.EfCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pms.Payrolls.Persistence;
using Microsoft.EntityFrameworkCore;
using Pms.Payrolls.Tests;
using Pms.Payrolls.Domain.Services;
using Pms.Payrolls.Domain;
using static Pms.Payrolls.Domain.Enums;

namespace Pms.Payrolls.ServiceLayer.EfCore.Tests
{
    public class PayrollProviderTests
    {
        private IDbContextFactory<PayrollDbContext> _factory;
        private IProvidePayrollService _payrollProvider;
        public PayrollProviderTests()
        {
            _factory = new PayrollDbContextFactoryFixture();
            _payrollProvider = new PayrollProvider(_factory);
        }
         
        [Theory]
        [InlineData(2022,"")]
        public void ShouldGetAccuratePayrollsByYearsCovoredAndCompanyId(int yearsCovered, string companyId)
        {
            _payrollProvider.GetPayrolls(yearsCovered, companyId);
        }

    }
}