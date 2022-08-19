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
    public class ThirteenthMonthExport
    {
        
        public void StartExport(IEnumerable<ThirteenthMonth> thirteenthMonths, int year, BankType bankType)
        {
            string startupPath = AppDomain.CurrentDomain.BaseDirectory;
            string filePath = $@"{startupPath}\EXPORT\13th Month";
            Directory.CreateDirectory(filePath);
            string filename = $"{bankType}_{year}-13th Month".AppendFile(filePath);
            
            IWorkbook workbook = new HSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("13th Month");

            int i = 0;
            IRow row = sheet.CreateRow(i);
            WriteHeader(row);
            foreach (ThirteenthMonth thirteenthMonth in thirteenthMonths)
            {
                i++; 
                row = sheet.CreateRow(i);
                WriteData(row, thirteenthMonth);
            }

            using (var nTemplateFile = new FileStream(filename, FileMode.Create, FileAccess.Write))
                workbook.Write(nTemplateFile);
        }


        private void WriteHeader(IRow row)
        {
            row.CreateCell(0).SetCellValue("EE ID");
            row.CreateCell(1).SetCellValue("Grand Total");
            row.CreateCell(2).SetCellValue("Amount Deducted");
            row.CreateCell(3).SetCellValue("13th Month");
        }

        private void WriteData(IRow row, ThirteenthMonth thirteenthMonth)
        {
            row.CreateCell(0).SetCellValue(thirteenthMonth.EEId);
            row.CreateCell(1).SetCellValue(thirteenthMonth.TotalRegPay.ToString("N2"));
            row.CreateCell(2).SetCellValue(thirteenthMonth.TotalAmountDeducted.ToString("N2"));
            row.CreateCell(3).SetCellValue(thirteenthMonth.Amount.ToString("N2"));
        }


    }
}