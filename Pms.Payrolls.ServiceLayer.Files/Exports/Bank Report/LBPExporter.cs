using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using Pms.Payrolls.Domain;
using Pms.Payrolls.Domain.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Pms.Payrolls.ServiceLayer.Files.Exports
{
    public class LBPExporter : IExportBankReportService
    {

        public void StartExport(IEnumerable<Payroll> payrolls, string cutoffId, string payrollCode)
        {
            Cutoff cutoff = new Cutoff(cutoffId);
            string startupPath = AppDomain.CurrentDomain.BaseDirectory;
            string filePath = $@"{startupPath}\EXPORT\{cutoffId}\{payrollCode}\BANK REPORT\LBP";
            Directory.CreateDirectory(filePath);
            string templatePath = $@"{startupPath}\TEMPLATES\TEMPLATE-LBP.xls";

            string filename = $"{payrollCode}_{cutoff.CutoffDate:yyyyMMdd}-LBP".AppendFile(filePath);
            File.Copy(templatePath, filename);

            payrolls = payrolls.OrderBy(p => p.EE.Fullname);
            IEnumerable<Payroll> validPayrolls = payrolls.Where(p => !(p.EE is null || p.EE.AccountNumber == "" || p.EE.CardNumber == ""));
            IEnumerable<Payroll> invalidPayrolls = payrolls.Where(p => p.EE is null || p.EE.AccountNumber == "" || p.EE.CardNumber == "");

            GenerateXls(filename, validPayrolls.ToArray(), invalidPayrolls.ToArray());
            GenerateCsvandDat(filename, validPayrolls.Count(), payrollCode);
        }

        private static void GenerateXls(string filename, Payroll[] payrolls, Payroll[] invalidPayrolls)
        {
            IWorkbook nWorkbook;
            using (var nTemplateFile = new FileStream(filename, FileMode.Open, FileAccess.ReadWrite))
                nWorkbook = new HSSFWorkbook(nTemplateFile);
            ISheet nSheet = nWorkbook.GetSheetAt(0);

            WritePayrollToOriginalSheet(payrolls, nSheet);
            nSheet = nWorkbook.GetSheetAt(1);
            WriteValidPayrollToSheet(payrolls, nSheet);
            nSheet = nWorkbook.GetSheetAt(2);
            WriteInvalidPayrollToSheet(invalidPayrolls, nSheet);

            using (var nReportFile = new FileStream(filename, FileMode.Open, FileAccess.Write))
                nWorkbook.Write(nReportFile);
        }
        private static void WritePayrollToOriginalSheet(Payroll[] validayrolls, ISheet sheet)
        {
            if (validayrolls.Length > 0)
            {
                IRow row;
                for (int i = 0; i < validayrolls.Length; i++)
                {
                    Payroll payroll = validayrolls[i];
                    row = sheet.GetRow(i + 2);
                    row.GetCell(0).SetCellValue(payroll.EE.CardNumber);
                    row.GetCell(1).SetCellValue(payroll.EE.AccountNumber);
                    row.GetCell(6).SetCellValue(payroll.NetPay * 100);
                }
            }
        }

        private static void WriteValidPayrollToSheet(Payroll[] validayrolls, ISheet sheet)
        {
            if (validayrolls.Length > 0)
            {
                IRow row;
                for (int i = 0; i < validayrolls.Length; i++)
                {
                    Payroll payroll = validayrolls[i];
                    row = sheet.CreateRow(i + 2);
                    row.CreateCell(0).SetCellValue(payroll.EEId);
                    row.CreateCell(1).SetCellValue(payroll.EE.Fullname);
                    row.CreateCell(2).SetCellValue(payroll.EE.CardNumber);
                    row.CreateCell(3).SetCellValue(payroll.EE.AccountNumber);
                    row.CreateCell(4).SetCellValue(payroll.NetPay);
                }

                row = sheet.CreateRow(validayrolls.Length + 2);
                row.CreateCell(0).SetCellValue("TOTAL");
                row.CreateCell(4).SetCellValue(validayrolls.Sum(p => p.NetPay));


                row = sheet.CreateRow(validayrolls.Length + 6);
                row.CreateCell(1).SetCellValue("Prepared By:");
                row.CreateCell(2).SetCellValue("Noted By:");
                row.CreateCell(3).SetCellValue("Approved By:");

                row = sheet.CreateRow(validayrolls.Length + 8);
                row.CreateCell(1).SetCellValue("");
                row.CreateCell(2).SetCellValue("Arlyn C. Esmenda");
                row.CreateCell(3).SetCellValue("Francis Ann B. Petilla");
            }
        }

        private static void WriteInvalidPayrollToSheet(Payroll[] invalidPayrolls, ISheet sheet)
        {
            if (invalidPayrolls.Length > 0)
            {
                IRow row;
                for (int i = 0; i < invalidPayrolls.Length; i++)
                {
                    Payroll payroll = invalidPayrolls[i];
                    row = sheet.CreateRow(i + 2);
                    row.CreateCell(0).SetCellValue(payroll.EEId);
                    if (payroll.EE is not null)
                    {
                        row.CreateCell(1).SetCellValue(payroll.EE.Fullname);
                        row.CreateCell(2).SetCellValue(payroll.EE.CardNumber);
                        row.CreateCell(3).SetCellValue(payroll.EE.AccountNumber);
                    }
                    row.CreateCell(4).SetCellValue(payroll.NetPay);
                }

                row = sheet.CreateRow(invalidPayrolls.Length + 3);
                row.CreateCell(0).SetCellValue("TOTAL");
                row.CreateCell(4).SetCellValue(invalidPayrolls.Sum(p => p.NetPay));
            }
        }



        private static void GenerateCsvandDat(string filename, int payrollCount, string payrollCode)
        {
            IWorkbook nWorkbook;
            using (var nTemplateFile = new FileStream(filename, FileMode.Open, FileAccess.ReadWrite))
                nWorkbook = new HSSFWorkbook(nTemplateFile);
            HSSFFormulaEvaluator formulator = new HSSFFormulaEvaluator(nWorkbook);
            ISheet nSheet = nWorkbook.GetSheetAt(0);

            string csvFilename = Path.ChangeExtension(filename, "csv");
            string headerInitial = "HD";
            string footerInitial = "FT";
            string datFileInitial = "TXU";
            string bankCode = "019372";
            DateTime timestamp = DateTime.Now;
            string versionNumber = "2.0";
            int totalRecords = payrollCount;

            using (var streamWriter = new StreamWriter(csvFilename))
            {
                streamWriter.WriteLine($"{headerInitial}{bankCode}{timestamp:ddMMyyyyHHmmss}{versionNumber}");

                for (int i = 0; i < totalRecords; i++)
                {
                    IRow row = nSheet.GetRow(i + 2);

                    string item = row.GetCell(0).GetValue(formulator);
                    for (int j = 1; j < 26; j++)
                        item += $"|{row.GetCell(j).GetValue(formulator)}";
                    streamWriter.WriteLine(item);
                }

                streamWriter.WriteLine($"{footerInitial}{totalRecords:000000000}");
            }

            string datFilename = $@"{Path.GetDirectoryName(filename)}\{payrollCode}_{datFileInitial}{bankCode}{timestamp:ddMMyyHHmmss}.dat";
            File.Copy(csvFilename, datFilename);

            if (!File.Exists(filename))
                throw new FileNotFoundException("Bank Report was not generated successfully.");
        }

        //private string GetValue(ICell cell, HSSFFormulaEvaluator formulator = null)
        //{
        //    switch (cell.CellType)
        //    {
        //        case CellType.Numeric:
        //            if (DateUtil.IsCellDateFormatted(cell))
        //            {
        //                DateTime date = cell.DateCellValue;
        //                ICellStyle style = cell.CellStyle;
        //                // Excel uses lowercase m for month whereas .Net uses uppercase
        //                string format = style.GetDataFormatString().Replace('m', 'M');
        //                return date.ToString(format);
        //            }
        //            else
        //            {
        //                double numericValue = cell.NumericCellValue;
        //                ICellStyle style = cell.CellStyle;
        //                string format = style.GetDataFormatString();
        //                if (format != "General")
        //                    return numericValue.ToString(format);
        //                return cell.NumericCellValue.ToString();
        //            }
        //        case CellType.Formula:
        //            return GetValue(formulator.EvaluateInCell(cell));
        //        default:
        //            return cell.StringCellValue;
        //    }
        //}





        //public static void ExportBankReport(ref List<Payroll> payArr, string filePath, DateTime payrollDate, string payrollCodes, string bankName, string bankCategory, string companyName)
        //{
        //    string excelTemplate = string.Format(@"{0}\templates\template-{1}.xls", AppDomain.CurrentDomain.BaseDirectory, bankName);
        //    string METROTACFile = filePath + @"\" + bankName;
        //    if (!File.Exists(excelTemplate))
        //    {
        //        return;
        //    }
        //    try
        //    {
        //        string filename = $"{companyName}-{payrollCodes}_{payrollDate:yyyyMMdd}-{bankName}".AppendFile(filePath);

        //        File.Copy(excelTemplate, filename);

        //        IWorkbook nWorkbook;
        //        using (var nTemplateFile = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.Read))
        //            nWorkbook = new HSSFWorkbook(nTemplateFile);

        //        if (bankName == "CHK")
        //            ExportDataCHECK(ref payArr, filename, nWorkbook);
        //        else
        //        {
        //            ISheet nSheet = nWorkbook.GetSheetAt(0);
        //            switch ((bankName + " " + bankCategory).Trim() ?? "")
        //            {
        //                case "UCPB CCARD":
        //                    ExportDataUCPB(ref payArr, nSheet);
        //                    break;
        //                case "CHINABANK CCARD":
        //                    ExportDataCHINABANK(ref payArr, nSheet);
        //                    break;
        //                case "ZEROS":
        //                    ExportDataZeros(ref payArr, nSheet);
        //                    break;
        //            }

        //            using (var nReportFile = new FileStream(filename, FileMode.Open, FileAccess.Write))
        //                nWorkbook.Write(nReportFile);
        //        }
        //    }

        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }
        //}

        //public static void ExportDataUCPB(ref List<Payroll> payArr, ISheet sheet)
        //{
        //    for (int i = 0, loopTo = payArr.Count - 1; i <= loopTo; i++)
        //    {
        //        var rec = payArr[i];
        //        IRow row = sheet.CreateRow(i);
        //        row.CreateCell(0).SetCellValue(rec.EE.FirstName);
        //        row.CreateCell(1).SetCellValue(rec.EE.LastName);
        //        row.CreateCell(2).SetCellValue(rec.EE.MiddleName);
        //        if (rec.EE.AccountNumber.Length == 14)
        //        {
        //            row.CreateCell(3).SetCellValue("5900010" + rec.EE.CardNumber);
        //        }
        //        else
        //        {
        //            row.CreateCell(3).SetCellValue(rec.EE.CardNumber);
        //        }
        //        row.CreateCell(4).SetCellValue(rec.NetPay);
        //    }
        //}

        //public static void ExportDataCHINABANK(ref List<Payroll> payArr, ISheet sheet)
        //{
        //    for (int i = 0, loopTo = payArr.Count - 1; i <= loopTo; i++)
        //    {
        //        var rec = payArr[i];
        //        IRow row = sheet.CreateRow(i + 5);
        //        row.CreateCell(3).SetCellValue(rec.EE.AccountNumber);
        //        row.CreateCell(4).SetCellValue(rec.EE.LastName);
        //        row.CreateCell(5).SetCellValue(rec.EE.FirstName);
        //        row.CreateCell(6).SetCellValue(rec.EE.MiddleName);
        //        row.CreateCell(7).SetCellValue(rec.NetPay);
        //    }
        //}


        //public static void ExportDataMETROPALO(ref List<Payroll> payArr, ISheet sheet)
        //{
        //    for (int i = 0, loopTo = payArr.Count - 1; i <= loopTo; i++)
        //    {
        //        var rec = payArr[i];
        //        IRow row = sheet.CreateRow(i);
        //        row.CreateCell(0).SetCellValue(rec.EE.LastName);
        //        row.CreateCell(1).SetCellValue(rec.EE.FirstName);
        //        row.CreateCell(2).SetCellValue(rec.EE.MiddleName);
        //        row.CreateCell(3).SetCellValue("756" + rec.EE.AccountNumber);
        //        row.CreateCell(4).SetCellValue(rec.NetPay);
        //    }
        //}

        //public static void ExportDataMETROTAC(ref List<Payroll> payArr, ISheet sheet)
        //{
        //    for (int i = 0, loopTo = payArr.Count - 1; i <= loopTo; i++)
        //    {
        //        var rec = payArr[i];
        //        IRow row = sheet.CreateRow(i);
        //        row.CreateCell(0).SetCellValue(rec.EE.LastName);
        //        row.CreateCell(1).SetCellValue(rec.EE.FirstName);
        //        row.CreateCell(2).SetCellValue(rec.EE.MiddleName);
        //        row.CreateCell(3).SetCellValue("525" + rec.EE.AccountNumber);
        //        row.CreateCell(4).SetCellValue(rec.NetPay);
        //    }
        //}

        //public static void ExportDataZeros(ref List<Payroll> payArr, ISheet sheet)
        //{
        //    IRow row = sheet.CreateRow(0);
        //    row.CreateCell(0).SetCellValue("IDNo");
        //    row.CreateCell(1).SetCellValue("Fullname");
        //    row.CreateCell(2).SetCellValue("Amount");

        //    for (int i = 0, loopTo = payArr.Count - 1; i <= loopTo; i++)
        //    {
        //        var rec = payArr[i];
        //        row = sheet.CreateRow(i + 1);
        //        row.CreateCell(0).SetCellValue(rec.EEId);
        //        row.CreateCell(1).SetCellValue(rec.EE.Fullname);
        //        row.CreateCell(2).SetCellValue(rec.NetPay);
        //    }
        //}

        //public static void ExportDataCHECK(ref List<Payroll> payArr, string filePath, IWorkbook workbook)
        //{
        //    try
        //    {
        //        ISheet xl200DOWNSheet = workbook.GetSheetAt(0);
        //        ISheet xl7500DOWNSheet = workbook.GetSheetAt(1);
        //        ISheet xl7500UPSheet = workbook.GetSheetAt(2);
        //        ISheet xl100KUPSheet = workbook.GetSheetAt(3);

        //        Payroll[] Records200DOWN = payArr
        //            .Where(p => p.NetPay <= 200)
        //            .Where(p => p.NetPay > 0)
        //            .ToArray();
        //        Payroll[] Records7500DOWN = payArr
        //            .Where(p => p.NetPay < 7500)
        //            .Where(p => p.NetPay > 200)
        //            .ToArray();
        //        Payroll[] Recordsd7500UP = payArr
        //            .Where(p => p.NetPay >= 7500)
        //            .ToArray();
        //        Payroll[] Records100KUP = payArr
        //            .Where(p => p.NetPay >= 100000)
        //            .ToArray();

        //        WriteSpecificReport(xl7500UPSheet, Recordsd7500UP);
        //        WriteSpecificReport(xl7500DOWNSheet, Records7500DOWN);
        //        WriteSpecificReport(xl200DOWNSheet, Records200DOWN);
        //        WriteSpecificReport(xl100KUPSheet, Records100KUP);

        //        using (var nNewPayreg = new FileStream(filePath, FileMode.Open, FileAccess.Write))
        //        {
        //            workbook.Write(nNewPayreg);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}

        //public static void WriteSpecificReport(ISheet sheet, Payroll[] payrollRecords)
        //{
        //    IRow row = sheet.CreateRow(0);
        //    row.CreateCell(0).SetCellValue("IDNo");
        //    row.CreateCell(1).SetCellValue("Fullname");
        //    row.CreateCell(2).SetCellValue("Amount");

        //    for (int i = 0, loopTo = payrollRecords.Count() - 1; i <= loopTo; i++)
        //    {
        //        var rec = payrollRecords[i];
        //        row = sheet.CreateRow(i + 1);
        //        row.CreateCell(0).SetCellValue(rec.EEId);
        //        row.CreateCell(1).SetCellValue(rec.EE.Fullname);
        //        row.CreateCell(2).SetCellValue(rec.NetPay);
        //    }
        //}





    }
}