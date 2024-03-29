﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;

namespace Pms.Payrolls.Persistence
{
    public class PayrollDbContextFactory : IDbContextFactory<PayrollDbContext>, IDesignTimeDbContextFactory<PayrollDbContext>
    {

        private string _connectionString { get; set; }
        private readonly bool _lazyLoad;

        public PayrollDbContextFactory(string connectionString, bool lazyLoad=false)
        {
            _connectionString = connectionString;
            _lazyLoad = lazyLoad;
        }

        public PayrollDbContextFactory() =>
            _connectionString = "server=localhost;database=payroll3Test_efdb;user=root;password=Soft1234;";

        public PayrollDbContext CreateDbContext()
        {
            DbContextOptions options = new DbContextOptionsBuilder()
                .UseLazyLoadingProxies(_lazyLoad)
                .UseMySQL(
                    _connectionString, 
                    options => options.MigrationsHistoryTable("payrollMigrationHistory")
                )
                .Options;
            return new PayrollDbContext(options);
        }

        public PayrollDbContext CreateDbContext(string[] args) => CreateDbContext();
    }
}
