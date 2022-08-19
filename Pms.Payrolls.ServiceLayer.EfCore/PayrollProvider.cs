using Microsoft.EntityFrameworkCore;
using Pms.Payrolls.Domain;
using Pms.Payrolls.Domain.Services;
using Pms.Payrolls.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using static Pms.Payrolls.Domain.Enums;

namespace Pms.Payrolls.ServiceLayer.EfCore
{
    public class PayrollProvider : IProvidePayrollService
    {
        private IDbContextFactory<PayrollDbContext> _factory;

        public PayrollProvider(IDbContextFactory<PayrollDbContext> factory)
        {
            _factory = factory;
        }

        public IEnumerable<Payroll> GetPayrolls(string cutoffId, BankType bank)
        {
            using PayrollDbContext context = _factory.CreateDbContext();
            return context.Payrolls
                .Where(p => p.CutoffId == cutoffId)
                .Where(p => p.Bank == bank)
                .ToList();
        }
        public IEnumerable<Payroll> GetPayrolls(int yearsCovered, BankType bank)
        {
            using PayrollDbContext context = _factory.CreateDbContext();
            return context.Payrolls
                .Where(p => p.YearCovered == yearsCovered)
                .Where(p => p.Bank == bank)
                .ToList();
        }

        public IEnumerable<Payroll> GetAllPayrolls()
        {
            using PayrollDbContext context = _factory.CreateDbContext();
            return context.Payrolls.ToList();
        }

    }
}
