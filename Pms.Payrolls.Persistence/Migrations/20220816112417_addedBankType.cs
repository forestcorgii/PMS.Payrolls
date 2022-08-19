﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace Pms.Payrolls.Persistence.Migrations
{
    public partial class addedBankType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Bank",
                table: "payroll",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Bank",
                table: "payroll");
        }
    }
}
