﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Pms.Payrolls.Persistence;

namespace Pms.Payrolls.Persistence.Migrations
{
    [DbContext(typeof(PayrollDbContext))]
    [Migration("20220816112417_addedBankType")]
    partial class addedBankType
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "5.0.17");

            modelBuilder.Entity("Pms.Payrolls.Domain.Payroll", b =>
                {
                    b.Property<string>("PayrollId")
                        .HasColumnType("VARCHAR(35)");

                    b.Property<double>("Adjust1Total")
                        .HasColumnType("DOUBLE(9,2)");

                    b.Property<double>("Adjust2Total")
                        .HasColumnType("DOUBLE(9,2)");

                    b.Property<int>("Bank")
                        .HasColumnType("int");

                    b.Property<string>("BankCategory")
                        .HasColumnType("VARCHAR(6)");

                    b.Property<string>("CutoffId")
                        .IsRequired()
                        .HasColumnType("VARCHAR(20)");

                    b.Property<string>("EEId")
                        .IsRequired()
                        .HasColumnType("VARCHAR(8)");

                    b.Property<double>("GovernmentTotal")
                        .HasColumnType("DOUBLE(9,2)");

                    b.Property<double>("GrossPay")
                        .HasColumnType("DOUBLE(9,2)");

                    b.Property<double>("NetPay")
                        .HasColumnType("DOUBLE(9,2)");

                    b.Property<string>("PayrollCode")
                        .HasColumnType("VARCHAR(6)");

                    b.Property<double>("RegPay")
                        .HasColumnType("DOUBLE(9,2)");

                    b.HasKey("PayrollId");

                    b.ToTable("payroll");
                });
#pragma warning restore 612, 618
        }
    }
}
