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
    public class AlphalistExporter
    {
        public void StartExport(IEnumerable<AlphalistDetail> alphalists, int year, string companyId,double minimumRate)
        {
            string startupPath = AppDomain.CurrentDomain.BaseDirectory;
            string filePath = $@"{startupPath}\EXPORT\ALPHALIST";
            Directory.CreateDirectory(filePath);
            string filename = $"{filePath}\\{companyId}_{year}-Alpha.xls";

            IWorkbook workbook = new HSSFWorkbook();

            ISheet sheet = workbook.CreateSheet("D1");
            WriteToSheet(alphalists.Where(a => a.ActualHourlyRate > minimumRate), sheet, AlphalistScheduleNumberChoices.D1);

            sheet = workbook.CreateSheet("D2");
            WriteToSheet(alphalists.Where(a => a.ActualHourlyRate <= minimumRate), sheet, AlphalistScheduleNumberChoices.D2);

            using (var nTemplateFile = new FileStream(filename, FileMode.Create, FileAccess.Write))
                workbook.Write(nTemplateFile);
        }



        public void WriteToSheet(IEnumerable<AlphalistDetail> alphalists, ISheet sheet, AlphalistScheduleNumberChoices type)
        {
            int i = -1;
            IRow row = sheet.CreateRow(append(ref i));
            WriteHeader(row);

            foreach (AlphalistDetail alpha in alphalists)
                WriteData(sheet.CreateRow(append(ref i)), alpha);
        }


        private void WriteHeader(IRow row)
        {
            int index = -1;
            row.CreateCell(append(ref index)).SetCellValue("ID");
            row.CreateCell(append(ref index)).SetCellValue("FIRST_NAME");
            row.CreateCell(append(ref index)).SetCellValue("LAST_NAME");
            row.CreateCell(append(ref index)).SetCellValue("MIDDLE_NAME");
            row.CreateCell(append(ref index)).SetCellValue("TIN");
            row.CreateCell(append(ref index)).SetCellValue("EMPLOYMENT_FROM");
            row.CreateCell(append(ref index)).SetCellValue("EMPLOYMENT_TO");
            row.CreateCell(append(ref index)).SetCellValue("FACTOR_USED");
            row.CreateCell(append(ref index)).SetCellValue("ACTUAL_AMT_WTHLD");
            row.CreateCell(append(ref index)).SetCellValue("PRES_TAXABLE_SALARIES");
            row.CreateCell(append(ref index)).SetCellValue("PRES_TAXABLE_13TH_MONTH");
            row.CreateCell(append(ref index)).SetCellValue("PRES_TAX_WTHLD");
            row.CreateCell(append(ref index)).SetCellValue("PRES_NONTAX_SALARIES");
            row.CreateCell(append(ref index)).SetCellValue("PRES_NONTAX_13TH_MONTH");
            row.CreateCell(append(ref index)).SetCellValue("PRES_NONTAX_SSS_GSIS_OTH_CONT");
            row.CreateCell(append(ref index)).SetCellValue("OVER_WTHLD");
            row.CreateCell(append(ref index)).SetCellValue("AMT_WTHLD_DEC");
            row.CreateCell(append(ref index)).SetCellValue("TAX_DUE");
            row.CreateCell(append(ref index)).SetCellValue("NET_TAXABLE_COMP_INCOME");
            row.CreateCell(append(ref index)).SetCellValue("GROSS_COMP_INCOME");
            row.CreateCell(append(ref index)).SetCellValue("PRES_NONTAX_DE_MINIMIS");
            row.CreateCell(append(ref index)).SetCellValue("PRES_TOTAL_COMP");
            row.CreateCell(append(ref index)).SetCellValue("PRES_TOTAL_NONTAX_COMP_INCOME");
            row.CreateCell(append(ref index)).SetCellValue("PRES_NONTAX_GROSS_COMP_INCOME");
            row.CreateCell(append(ref index)).SetCellValue("PRES_NONTAX_BASIC_SMW_DAY");
            row.CreateCell(append(ref index)).SetCellValue("PRES_NONTAX_BASIC_SMW_MONTH");
            row.CreateCell(append(ref index)).SetCellValue("PRES_NONTAX_BASIC_SMW_YEAR");
            row.CreateCell(append(ref index)).SetCellValue("PRES_NONTAX_HOLIDAY_PAY");
            row.CreateCell(append(ref index)).SetCellValue("PRES_NONTAX_OVERTIME_PAY");
            row.CreateCell(append(ref index)).SetCellValue("PRES_NONTAX_NIGHT_DIFF");
            row.CreateCell(append(ref index)).SetCellValue("PRES_NONTAX_HAZARD_PAY");
            row.CreateCell(append(ref index)).SetCellValue("NONTAX_BASIC_SAL");
            row.CreateCell(append(ref index)).SetCellValue("TAX_BASIC_SAL");
            row.CreateCell(append(ref index)).SetCellValue("NATIONALITY");
            row.CreateCell(append(ref index)).SetCellValue("REASON_SEPARATION");
            row.CreateCell(append(ref index)).SetCellValue("EMPLOYMENT_STATUS");
            row.CreateCell(append(ref index)).SetCellValue("December");
            row.CreateCell(append(ref index)).SetCellValue("Final");
        }

        private void WriteData(IRow row, AlphalistDetail alpha)
        {
            int index = -1;
            row.CreateCell(append(ref index)).SetCellValue(alpha.EEId);
            row.CreateCell(append(ref index)).SetCellValue(alpha.FirstName);
            row.CreateCell(append(ref index)).SetCellValue(alpha.LastName);
            row.CreateCell(append(ref index)).SetCellValue(alpha.MiddleName);
            row.CreateCell(append(ref index)).SetCellValue(alpha.Tin);
            row.CreateCell(append(ref index)).SetCellValue(alpha.StartDate.ToString("yyyy-MM-dd"));
            row.CreateCell(append(ref index)).SetCellValue(alpha.ResignationDate.ToString("yyyy-MM-dd"));
            row.CreateCell(append(ref index)).SetCellValue(alpha.FactorUsed);
            row.CreateCell(append(ref index)).SetCellValue(alpha.ActualAmountWithheld);
            row.CreateCell(append(ref index)).SetCellValue(alpha.PresentTaxableSalary);
            row.CreateCell(append(ref index)).SetCellValue(alpha.PresentTaxable13thMonth);
            row.CreateCell(append(ref index)).SetCellValue(alpha.PresentTaxWithheld);
            row.CreateCell(append(ref index)).SetCellValue(alpha.PresentNonTaxableSalary);
            row.CreateCell(append(ref index)).SetCellValue(alpha.PresentNonTaxable13thMonth);
            row.CreateCell(append(ref index)).SetCellValue(alpha.PresentNonTaxableSssGsisOtherContribution);
            row.CreateCell(append(ref index)).SetCellValue(alpha.OverWithheld);
            row.CreateCell(append(ref index)).SetCellValue(alpha.AmmountWithheldOnDecember);
            row.CreateCell(append(ref index)).SetCellValue(alpha.TaxDue);
            row.CreateCell(append(ref index)).SetCellValue(alpha.NetTaxableCompensationIncome);
            row.CreateCell(append(ref index)).SetCellValue(alpha.GrossCompensationIncome);
            row.CreateCell(append(ref index)).SetCellValue(alpha.PresentNonTaxableDeMinimis);
            row.CreateCell(append(ref index)).SetCellValue(alpha.PresentTotalCompensation);
            row.CreateCell(append(ref index)).SetCellValue(alpha.PresentTotalNonTaxableCompensationIncome);
            row.CreateCell(append(ref index)).SetCellValue(alpha.PresentNonTaxableGrossCompensationIncome);
            row.CreateCell(append(ref index)).SetCellValue(alpha.PresentNonTaxableBasicSmwDay);
            row.CreateCell(append(ref index)).SetCellValue(alpha.PresentNonTaxableBasicSmwMonth);
            row.CreateCell(append(ref index)).SetCellValue(alpha.PresentNonTaxableBasicSmwYear);
            row.CreateCell(append(ref index)).SetCellValue(alpha.PresentNonTaxableHolidayPay);
            row.CreateCell(append(ref index)).SetCellValue(alpha.PresentNonTaxableOvertimePay);
            row.CreateCell(append(ref index)).SetCellValue(alpha.PresentNonTaxableNightDifferential);
            row.CreateCell(append(ref index)).SetCellValue(alpha.PresentNonTaxableHazardPay);
            row.CreateCell(append(ref index)).SetCellValue(alpha.NonTaxableBasicSalary);
            row.CreateCell(append(ref index)).SetCellValue(alpha.TaxableBasicSalary);
            row.CreateCell(append(ref index)).SetCellValue(alpha.Nationality);
            row.CreateCell(append(ref index)).SetCellValue(alpha.ReasonForSeparation);
            row.CreateCell(append(ref index)).SetCellValue(alpha.EmploymentStatus);
            row.CreateCell(append(ref index)).SetCellValue(alpha.December);
            row.CreateCell(append(ref index)).SetCellValue(alpha.OverWithheld);
        }


        private static int append(ref int index)
        {
            index++;
            return index;
        }
    }
}