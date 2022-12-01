using Microsoft.EntityFrameworkCore;
using Pms.Payrolls.Domain;
using Pms.Payrolls.Domain.Services;
using Pms.Payrolls.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pms.Payrolls.ServiceLayer.EfCore
{
    public class PayrollManager : IManagePayrollService
    {
        private IDbContextFactory<PayrollDbContext> _factory;

        public PayrollManager(IDbContextFactory<PayrollDbContext> factory) =>
            _factory = factory;


        public void ValidatePayroll(Payroll payroll)
        {

        }

        public void SavePayroll(Payroll payroll)
        {
            payroll.Validate();

            using PayrollDbContext context = _factory.CreateDbContext();
            if (context.Payrolls.Any(p => p.PayrollId == payroll.PayrollId))
                context.Update(payroll);
            else
                context.Add(payroll);
            context.SaveChanges();
        }
    }
}
