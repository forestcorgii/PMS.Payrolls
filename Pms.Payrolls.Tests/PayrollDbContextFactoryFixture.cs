using Microsoft.EntityFrameworkCore;
using Pms.Payrolls.Domain;
using Pms.Payrolls.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Pms.Payrolls.Domain.Enums;
using static Pms.Payrolls.Tests.Seeder;

namespace Pms.Payrolls.Tests
{
    class PayrollDbContextFactoryFixture : IDbContextFactory<PayrollDbContext>
    {
        private const string ConnectionString = "server=localhost;database=payroll3Test_efdb;user=root;password=Soft1234;";

        private static bool _databaseInitialized;

        public PayrollDbContextFactoryFixture()
        {
            CreateFactory();
            if (!_databaseInitialized)
            {
                using (var context = CreateDbContext())
                {
                    context.Database.Migrate();
                    TrySeeding(context);
                }

                _databaseInitialized = true;
            }
        }

        private void TrySeeding(PayrollDbContext context)
        {
            Cutoff cutoff = new();
            List<Payroll> payrolls = new()
            {
                GenerateSeedPayroll("DYYJ", cutoff.CutoffId, BankType.LBP, 10000, 10000, 10000, 1000, -500, -1000),
                GenerateSeedPayroll("DYYZ", cutoff.CutoffId, BankType.LBP, 10000, 10000, 10000, 1000, -500, -1000),
                GenerateSeedPayroll("DYYN", cutoff.CutoffId, BankType.LBP, 10000, 10000, 10000, 1000, -500, -1000),
                GenerateSeedPayroll("DYYK", cutoff.CutoffId, BankType.LBP, 10000, 10000, 10000, 1000, -500, -1000),
            };

            foreach (Payroll payroll in payrolls)
            {
                if (!context.Payrolls.Any(b => b.PayrollId == payroll.PayrollId))
                    context.Add(payroll);
            }
            context.SaveChanges();
        }

       

        public PayrollDbContextFactory Factory;
        public void CreateFactory()
            => Factory = new PayrollDbContextFactory(ConnectionString);

        public PayrollDbContext CreateDbContext()
            => Factory.CreateDbContext();
    }

}
