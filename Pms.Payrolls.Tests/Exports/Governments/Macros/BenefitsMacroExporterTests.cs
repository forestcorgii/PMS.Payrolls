using Xunit;
using Pms.Payrolls.ServiceLayer.Files.Exports.Governments.Macros;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pms.Payrolls.Domain;
using Microsoft.EntityFrameworkCore;
using Pms.Payrolls.Persistence;
using Pms.Payrolls.Domain.Services;
using Pms.Payrolls.Tests;
using Pms.Payrolls.ServiceLayer.EfCore;

namespace Pms.Payrolls.ServiceLayer.Files.Exports.Governments.Macros.Tests
{
    public class BenefitsMacroExporterTests
    {
        private IDbContextFactory<PayrollDbContext> _factory;
        private IProvidePayrollService _payrollProvider;

        public BenefitsMacroExporterTests()
        {
            _factory = new PayrollDbContextFactoryFixture();
            _payrollProvider = new PayrollProvider(_factory);
        }


        [Fact()]
        public void if_it_does_not_throw_exception()
        {
            string cutoffId = "2207-2";
            string companyId = "MIDCSI00";
            IEnumerable<Payroll> payrolls = _payrollProvider.GetPayrollsByCcompany(cutoffId, companyId);

            BenefitsMacroExporter exporter = new(new Cutoff(cutoffId), companyId);
            exporter.StartExport(payrolls.ToArray());
        }

        [Fact()]
        public void if_b_does_not_throw_exception()
        {
            string cutoffId = "2207-2";
            string companyId = "MIDCSI00";
            IEnumerable<Payroll> payrolls = _payrollProvider.GetPayrollsByCcompany(cutoffId, companyId);

            BenefitsBMacroExporter exporter = new(new Cutoff(cutoffId), companyId);
            exporter.StartExport(payrolls.ToArray());
        }
         
    }
}