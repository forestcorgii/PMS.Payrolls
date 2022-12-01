using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using Pms.Payrolls.Domain;
using Pms.Payrolls.Domain.Services;
using Pms.Payrolls.Domain.SupportTypes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static Pms.Payrolls.Domain.Enums;

namespace Pms.Payrolls.ServiceLayer.Files.Exports
{
    public class AlphalistVerifierExporter
    {
        public void StartExport(IEnumerable<IEnumerable<Payroll>> employeePayrolls, int year, string companyId)
        {
            string startupPath = AppDomain.CurrentDomain.BaseDirectory;
            string filePath = $@"{startupPath}\EXPORT\ALPHALIST";
            Directory.CreateDirectory(filePath);
            string filename = $"{companyId}_{year}-Alpha Verifier";

            IWorkbook workbook = new HSSFWorkbook();

            ISheet sheet = workbook.CreateSheet("ALL");
            WriteToSheet(employeePayrolls, sheet);

            using (var nTemplateFile = new FileStream(filename, FileMode.Create, FileAccess.Write))
                workbook.Write(nTemplateFile);
        }


        private void WriteToSheet(IEnumerable<IEnumerable<Payroll>> employeePayrolls, ISheet sheet)
        {
            int rowIndex = -1;
            IRow row = sheet.CreateRow(append(ref rowIndex));
            WriteHeader(row);

            foreach (IEnumerable<Payroll> employeePayroll in employeePayrolls)
            {
                Payroll temp = employeePayroll.First();
                WriteEmployeeData(sheet.CreateRow(append(ref rowIndex)), temp.EEId, temp.EE);

                foreach (Payroll payroll in employeePayroll)
                    WriteData(sheet.CreateRow(append(ref rowIndex)), payroll);

                append(ref rowIndex);
                WriteTotal(sheet.CreateRow(append(ref rowIndex)), employeePayroll);
                append(ref rowIndex);
                append(ref rowIndex);
            }
        }

        private void WriteHeader(IRow row)
        {
            int columnIndex = -1;
            row.CreateCell(append(ref columnIndex)).SetCellValue("CUTOFF DATE");
            row.CreateCell(append(ref columnIndex)).SetCellValue("RATE");
            row.CreateCell(append(ref columnIndex)).SetCellValue("REG HRS");
            row.CreateCell(append(ref columnIndex)).SetCellValue("OT");

            row.CreateCell(append(ref columnIndex)).SetCellValue("R_OT");
            row.CreateCell(append(ref columnIndex)).SetCellValue("H_OT");
            row.CreateCell(append(ref columnIndex)).SetCellValue("ND");
            
            row.CreateCell(append(ref columnIndex)).SetCellValue("TARDY");
            
            row.CreateCell(append(ref columnIndex)).SetCellValue("SSS");
            row.CreateCell(append(ref columnIndex)).SetCellValue("PAGIBIG");
            row.CreateCell(append(ref columnIndex)).SetCellValue("PHIC");
            row.CreateCell(append(ref columnIndex)).SetCellValue("TAX");
            row.CreateCell(append(ref columnIndex)).SetCellValue("REG PAY");
            row.CreateCell(append(ref columnIndex)).SetCellValue("REG PAY_13th MONTH");
            row.CreateCell(append(ref columnIndex)).SetCellValue("GROSS PAY");
            row.CreateCell(append(ref columnIndex)).SetCellValue("NET PAY");
        }
        private void WriteEmployeeData(IRow row, string eeId, EmployeeView employee)
        {
            int columnIndex = -1;
            row.CreateCell(append(ref columnIndex)).SetCellValue(eeId);
            if (employee is not null)
            {
                row.CreateCell(append(ref columnIndex)).SetCellValue(employee.Fullname);
                row.CreateCell(append(ref columnIndex)).SetCellValue(employee.TIN);
            }
        }
        private void WriteData(IRow row, Payroll payroll)
        {
            int columnIndex = -1;
            row.CreateCell(append(ref columnIndex)).SetCellValue(payroll.Cutoff.CutoffDate.ToString("MMM dd, yyyy"));
            row.CreateCell(append(ref columnIndex)).SetCellValue(payroll.Rate);
            row.CreateCell(append(ref columnIndex)).SetCellValue(payroll.RegHours);
            row.CreateCell(append(ref columnIndex)).SetCellValue(payroll.Overtime);

            row.CreateCell(append(ref columnIndex)).SetCellValue(payroll.RestDayOvertime);
            row.CreateCell(append(ref columnIndex)).SetCellValue(payroll.HolidayOvertime);
            row.CreateCell(append(ref columnIndex)).SetCellValue(payroll.NightDifferential);

            row.CreateCell(append(ref columnIndex)).SetCellValue(payroll.AbsTar);

            row.CreateCell(append(ref columnIndex)).SetCellValue(payroll.EmployeeSSS);
            row.CreateCell(append(ref columnIndex)).SetCellValue(payroll.EmployeePagibig);
            row.CreateCell(append(ref columnIndex)).SetCellValue(payroll.EmployeePhilHealth);
            row.CreateCell(append(ref columnIndex)).SetCellValue(payroll.WithholdingTax);
            row.CreateCell(append(ref columnIndex)).SetCellValue(payroll.RegularPay);
            row.CreateCell(append(ref columnIndex)).SetCellValue(payroll.AdjustedRegPay);
            row.CreateCell(append(ref columnIndex)).SetCellValue(payroll.GrossPay);
            row.CreateCell(append(ref columnIndex)).SetCellValue(payroll.NetPay);
        }

        private void WriteTotal(IRow row, IEnumerable<Payroll> employeePayroll)
        {
            int columnIndex = -1;
            row.CreateCell(append(ref columnIndex)).SetCellValue("TOTAL");
            row.CreateCell(append(ref columnIndex)).SetCellValue("");
            row.CreateCell(append(ref columnIndex)).SetCellValue("");
            row.CreateCell(append(ref columnIndex)).SetCellValue("");

            row.CreateCell(append(ref columnIndex)).SetCellValue("");
            row.CreateCell(append(ref columnIndex)).SetCellValue("");
            row.CreateCell(append(ref columnIndex)).SetCellValue("");

            row.CreateCell(append(ref columnIndex)).SetCellValue("");

            row.CreateCell(append(ref columnIndex)).SetCellValue(employeePayroll.Sum(p => p.EmployeeSSS));
            row.CreateCell(append(ref columnIndex)).SetCellValue(employeePayroll.Sum(p => p.EmployeePagibig));
            row.CreateCell(append(ref columnIndex)).SetCellValue(employeePayroll.Sum(p => p.EmployeePhilHealth));
            row.CreateCell(append(ref columnIndex)).SetCellValue(employeePayroll.Sum(p => p.WithholdingTax));
            row.CreateCell(append(ref columnIndex)).SetCellValue(employeePayroll.Sum(p => p.RegularPay));
            row.CreateCell(append(ref columnIndex)).SetCellValue(employeePayroll.Sum(p => p.AdjustedRegPay));
            row.CreateCell(append(ref columnIndex)).SetCellValue(employeePayroll.Sum(p => p.GrossPay));
            row.CreateCell(append(ref columnIndex)).SetCellValue(employeePayroll.Sum(p => p.NetPay));
        }


        private static int append(ref int index)
        {
            index++;
            return index;
        }
    }
}