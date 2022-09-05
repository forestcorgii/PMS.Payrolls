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
    public class MBExporter : IExportBankReportService
    {

        public void StartExport(IEnumerable<Payroll> payrolls, string cutoffId, string payrollCode)
        {
            Cutoff cutoff = new Cutoff(cutoffId);
            string startupPath = AppDomain.CurrentDomain.BaseDirectory;
            string filePath = $@"{startupPath}\EXPORT\{cutoffId}\{payrollCode}\BANK REPORT\CBC";
            Directory.CreateDirectory(filePath);
            string templatePath = $@"{startupPath}\TEMPLATES\TEMPLATE-CBC.xls";

            string filename = $"{payrollCode}_{cutoff.CutoffDate:yyyyMMdd}-LBP".AppendFile(filePath);
            File.Copy(templatePath, filename);

            payrolls = payrolls.OrderBy(p => p.EE.Fullname);
            IEnumerable<Payroll> validPayrolls = payrolls.Where(p => !p.IsReadyForExport()); 

            GenerateXls(filename, validPayrolls.ToArray()); 
        }

        private static void GenerateXls(string filename, Payroll[] payrolls)
        {
            IWorkbook nWorkbook;
            using (var nTemplateFile = new FileStream(filename, FileMode.Open, FileAccess.ReadWrite))
                nWorkbook = new HSSFWorkbook(nTemplateFile);
            ISheet nSheet = nWorkbook.GetSheetAt(0);

            WritePayrollToOriginalSheet(payrolls, nSheet);

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
                    row.GetCell(3).SetCellValue(payroll.EE.AccountNumber);
                    row.GetCell(4).SetCellValue(payroll.EE.LastName);
                    row.GetCell(5).SetCellValue(payroll.EE.FirstName);
                    row.GetCell(6).SetCellValue(payroll.EE.MiddleName);
                    row.GetCell(7).SetCellValue(payroll.NetPay );
                }
            }
        }

        

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