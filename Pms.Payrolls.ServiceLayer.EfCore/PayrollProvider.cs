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

        public IEnumerable<Payroll> GetAllPayrolls()
        {
            using PayrollDbContext context = _factory.CreateDbContext();
            return context.Payrolls.ToList();
        }


        public IEnumerable<Payroll> GetPayrolls(string cutoffId)
        {
            using PayrollDbContext context = _factory.CreateDbContext();
            return context.Payrolls
                .Include(p => p.EE)
                .Include(p => p.TS)
                .Where(p => p.CutoffId == cutoffId)
                .ToList();
        }
        public IEnumerable<Payroll> GetPayrolls(string cutoffId, BankChoices bank)
        {
            using PayrollDbContext context = _factory.CreateDbContext();
            return context.Payrolls
                .Include(p => p.EE)
                .Include(p => p.TS)
                .Where(p => p.CutoffId == cutoffId)
                .Where(p => p.EE.Bank == bank)
                .ToList();
        }
        public IEnumerable<Payroll> GetPayrolls(int yearsCovered, BankChoices bank)
        {
            using PayrollDbContext context = _factory.CreateDbContext();
            return context.Payrolls
                .Include(p => p.EE)
                .Include(p => p.TS)
                .Where(p => p.YearCovered == yearsCovered)
                .Where(p => p.EE.Bank == bank)
                .ToList();
        }
        public IEnumerable<Payroll> GetPayrolls(int yearsCovered, string companyId)
        {
            using PayrollDbContext context = _factory.CreateDbContext();
            return context.Payrolls
                .Include(p => p.EE)
                .Include(p => p.TS)
                .Where(p => p.YearCovered == yearsCovered)
                .Where(p => p.CompanyId == companyId)
                .ToList();
        }
        public IEnumerable<Payroll> GetPayrolls(string cutoffId, string payrollCode, BankChoices bankType)
        {
            using PayrollDbContext context = _factory.CreateDbContext();
            return context.Payrolls
                .Include(p => p.EE)
                .Include(p => p.TS)
                .Where(p => p.CutoffId == cutoffId)
                .Where(p => p.PayrollCode == payrollCode)
                .Where(p => p.EE.Bank == bankType)
                .ToList();
        }
        public IEnumerable<Payroll> GetPayrolls(string cutoffId, string payrollCode)
        {
            using PayrollDbContext context = _factory.CreateDbContext();
            return context.Payrolls
                .Include(p => p.EE)
                .Include(p => p.TS)
                .Where(p => p.CutoffId == cutoffId)
                .Where(p => p.PayrollCode == payrollCode)
                .ToList();
        }


        public IEnumerable<Payroll> GetNoEEPayrolls()
        {
            PayrollDbContext Context = _factory.CreateDbContext();
            IEnumerable<Payroll> validPayrolls = Context.Payrolls
                .Include(ts => ts.EE);
            IEnumerable<Payroll> payrolls = Context.Payrolls;
            payrolls = payrolls.Except(validPayrolls);
            Console.WriteLine(payrolls.Count());

            return payrolls;
        }



    }
}
