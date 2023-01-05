using Microsoft.EntityFrameworkCore;
using Pms.Payrolls.Domain;
using Pms.Payrolls.Domain.Services;
using Pms.Payrolls.Domain.SupportTypes;
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
            return context.Payrolls
                .Include(p => p.EE)
                .ToList();
        }


        public IEnumerable<Payroll> GetPayrolls(string cutoffId)
        {
            using PayrollDbContext context = _factory.CreateDbContext();
            return context.Payrolls
                .Include(p => p.EE)
                .Where(p => p.CutoffId == cutoffId)
                .ToList();
        }
        public IEnumerable<Payroll> GetPayrolls(string cutoffId, string payrollCode)
        {
            using PayrollDbContext context = _factory.CreateDbContext();
            return context.Payrolls
                .Include(p => p.EE)
                .Where(p => p.CutoffId == cutoffId)
                .Where(p => p.PayrollCode == payrollCode)
                .ToList();
        }
        public IEnumerable<Payroll> GetPayrolls(int yearsCovered, string companyId)
        {
            using PayrollDbContext context = _factory.CreateDbContext();
            return context.Payrolls
                .Include(p => p.EE).ToList()
                .Where(p =>
                    p.YearCovered == yearsCovered ||
                    p.Cutoff.CutoffDate.Year == yearsCovered
                )
                .Where(p => p.CompanyId == companyId)
                .ToList();
        }

        public IEnumerable<Payroll> GetPayrollsByCcompany(string cutoffId, string CompanyId)
        {
            using PayrollDbContext context = _factory.CreateDbContext();
            return context.Payrolls
                .Include(p => p.EE)
                .Where(p => p.CutoffId == cutoffId)
                .Where(p => p.CompanyId == CompanyId)
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

        public IEnumerable<MonthlyPayroll> GetMonthlyPayrolls(int month, string payrollCode)
        {
            using PayrollDbContext context = _factory.CreateDbContext();
            List<Payroll> payrolls = context.Payrolls.Include(p => p.EE).ToList();

            List<MonthlyPayroll> mpayrolls = payrolls.Where(p => p.Cutoff.CutoffDate.Month == month)
            .Where(p => p.CompanyId == payrollCode)
            .GroupBy(p => p.EEId)
            .Where(p => p.Any(p => p.Cutoff.CutoffDate.Day >= 28))
            .Select(p => new MonthlyPayroll(p.ToArray()))
            .ToList();

            return mpayrolls;
        }


    }
}
