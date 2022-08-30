using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pms.Payrolls.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pms.Payrolls.Persistence
{
    public class TimesheetViewConfiguration : IEntityTypeConfiguration<TimesheetView>
    {
        public void Configure(EntityTypeBuilder<TimesheetView> builder)
        {
            builder.ToView("timesheet").HasKey(ts => ts.TimesheetId);

            builder.Property(cc => cc.TimesheetId).HasColumnName("Id").HasColumnType("VARCHAR(35)").IsRequired();
            //builder.Property(cc => cc.EEId).HasColumnType("VARCHAR(8)").IsRequired();
            //builder.Property(cc => cc.PayrollCode).HasColumnType("VARCHAR(6)");
            //builder.Property(cc => cc.BankCategory).HasColumnType("VARCHAR(6)");
            //builder.Property(cc => cc.CutoffId).HasColumnType("VARCHAR(6)").IsRequired();
            //builder.Property(cc => cc.TotalHours).HasColumnType("DOUBLE(6,2)").IsRequired();
            //builder.Property(cc => cc.TotalOT).HasColumnType("DOUBLE(6,2)").IsRequired();
            //builder.Property(cc => cc.TotalRDOT).HasColumnType("DOUBLE(6,2)").IsRequired();
            //builder.Property(cc => cc.TotalHOT).HasColumnType("DOUBLE(6,2)").IsRequired();
            //builder.Property(cc => cc.TotalTardy).HasColumnType("DOUBLE(6,2)").IsRequired();
            //builder.Property(cc => cc.TotalND).HasColumnType("DOUBLE(6,2)").IsRequired();
        }
    }
}