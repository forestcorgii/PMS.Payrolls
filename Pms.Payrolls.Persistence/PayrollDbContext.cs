using Microsoft.EntityFrameworkCore;
using Pms.Payrolls.Domain;
using System;

namespace Pms.Payrolls.Persistence
{
    public class PayrollDbContext : DbContext
    {
        public DbSet<Payroll> Payrolls => Set<Payroll>();
        public DbSet<EmployeeView> Employees => Set<EmployeeView>();
        public DbSet<TimesheetView> Timesheets => Set<TimesheetView>();
        public DbSet<Company> Companies => Set<Company>();

        public PayrollDbContext(DbContextOptions options) : base(options) { }
         
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PayrollConfiguration());
            modelBuilder.ApplyConfiguration(new EmployeeViewConfiguration());
            modelBuilder.ApplyConfiguration(new TimesheetViewConfiguration());
            modelBuilder.ApplyConfiguration(new CompanyConfiguration());
        }

    }
}
