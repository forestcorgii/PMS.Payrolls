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
    public class LBPExport 
    {

        public void StartExport(IEnumerable<Payroll> payrolls, string cutoffId, string companyName)
        {
            Cutoff cutoff = new Cutoff(cutoffId);
            string startupPath = AppDomain.CurrentDomain.BaseDirectory;
            string filePath = $@"{startupPath}\EXPORT\LBP";
            Directory.CreateDirectory(filePath);
            string templatePath = $@"{startupPath}\TEMPLATES\TEMPLATE-LBP.xls";

            string filename = $"{companyName}_{cutoff.CutoffDate:yyyyMMdd}-LBP".AppendFile(filePath);
            File.Copy(templatePath, filename);

            GenerateXls(filename, payrolls.ToArray());

            GenerateCsvandDat(filename, payrolls.Count());
        }   

        private void GenerateXls(string filename, Payroll[] payrolls)
        {
            IWorkbook nWorkbook;
            using (var nTemplateFile = new FileStream(filename, FileMode.Open, FileAccess.ReadWrite))
                nWorkbook = new HSSFWorkbook(nTemplateFile);
            ISheet nSheet = nWorkbook.GetSheetAt(0);

            for (int i = 0; i < payrolls.Length; i++)
            {
                Payroll payroll = payrolls[i];
                IRow row = nSheet.GetRow(i + 2);
                if (payroll.EE is not null)
                {
                    row.CreateCell(0).SetCellValue(payroll.EE.CardNumber);
                    row.CreateCell(1).SetCellValue(payroll.EE.AccountNumber);
                }
                else
                {
                    row.CreateCell(0).SetCellValue(0);
                    row.CreateCell(1).SetCellValue(0);
                }
                row.CreateCell(6).SetCellValue(payroll.NetPay);
            }

            using (var nReportFile = new FileStream(filename, FileMode.Open, FileAccess.Write))
                nWorkbook.Write(nReportFile);

        }


        private void GenerateCsvandDat(string filename, int payrollCount)
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
            string timestamp = DateTime.Now.ToString("ddMMYYYYHHmmss");
            string versionNumber = "2.0";
            int totalRecords = payrollCount;

            using (var streamWriter = new StreamWriter(csvFilename))
            {
                streamWriter.WriteLine($"{headerInitial}{bankCode}{timestamp}{versionNumber}");

                for (int i = 0; i < totalRecords; i++)
                {
                    IRow row = nSheet.GetRow(i + 2);

                    string item = row.GetCell(0).GetValue( formulator);
                    for (int j = 1; j < 26; j++)
                        item += $"|{row.GetCell(j).GetValue(formulator)}";
                    streamWriter.WriteLine(item);
                }

                streamWriter.WriteLine($"{footerInitial}{totalRecords:000000000}");
            }

            string datFilename = $@"{Path.GetDirectoryName(filename)}\{datFileInitial}{bankCode}{timestamp}.dat";
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