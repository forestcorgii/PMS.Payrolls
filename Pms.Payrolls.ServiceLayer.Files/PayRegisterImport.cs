using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using Pms.Payrolls.Domain;
using Pms.Payrolls.Domain.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Pms.Payrolls.ServiceLayer.Files
{
    public class PayRegisterImport : IImportPayrollService
    {
        public void ValidatePayRegisterFile()
        {
            throw new NotImplementedException();
        }


        public IEnumerable<Payroll> StartImport(string payRegisterFilePath)
        {
            List<Payroll> payrolls = new();
            IWorkbook nWorkBook;
            using (var nNewPayreg = new FileStream(payRegisterFilePath, FileMode.Open, FileAccess.Read))
                nWorkBook = new HSSFWorkbook(nNewPayreg);
            HSSFFormulaEvaluator formulator = new HSSFFormulaEvaluator(nWorkBook);

            var nSheet = nWorkBook.GetSheetAt(0);

            int idIdx = FindHeaderColumnIndex("ID", nSheet);
            int nameIdx = FindHeaderColumnIndex("NAME", nSheet);//ABS_TAR
            int regHrsdx = FindHeaderColumnIndex("REG", nSheet);
            int abstarIdx = FindHeaderColumnIndex("ABS_TAR", nSheet);
            int grossIdx = FindHeaderColumnIndex("GROSS", nSheet);
            int regPayIdx = FindHeaderColumnIndex("REG_PAY", nSheet);
            int netPayIdx = FindHeaderColumnIndex("NET", nSheet);

            DateTime payrollDate = FindPayrollDate(nSheet);
            Cutoff cutoff = new Cutoff(payrollDate);

            for (int i = 11, loopTo = nSheet.LastRowNum; i <= loopTo; i++)
            {
                var row = nSheet.GetRow(i);
                if (row is not null)
                {
                    string employee_id = "";
                    if (idIdx > 0)
                    {
                        if (row.GetCell(idIdx) is null)
                            continue;
                        employee_id = row.GetCell(idIdx).StringCellValue.Trim();
                    }
                    else if (nameIdx > 0)
                    {
                        var name_args = ParseEmployeeDetail(row, nameIdx);
                        if (name_args is null)
                            continue;
                        employee_id = name_args[1].Trim();
                    }


                    var newPayroll = new Payroll()
                    {
                        EEId = employee_id,
                        CutoffId = cutoff.CutoffId,
                        YearCovered = cutoff.YearCovered,
                    };

                    newPayroll.RegPay = double.Parse(row.GetCell(regPayIdx).GetValue(formulator));
                    newPayroll.RegHours = double.Parse(row.GetCell(regHrsdx).GetValue(formulator));
                    newPayroll.AbsTar = double.Parse(row.GetCell(abstarIdx).GetValue(formulator));
                    newPayroll.GrossPay = double.Parse(row.GetCell(grossIdx).GetValue(formulator));
                    newPayroll.NetPay = double.Parse(row.GetCell(netPayIdx).GetValue(formulator));
                    newPayroll.PayrollId = Payroll.GenerateId(newPayroll);


                    payrolls.Add(newPayroll);
                }
            }
            return payrolls;
        }


        private static int FindHeaderColumnIndex(string header, ISheet sheet)
        {
            foreach (IRow row in new[] { sheet.GetRow(0), sheet.GetRow(1), sheet.GetRow(2) })
            {
                if (row is not null)
                    foreach (var cell in row.Cells)
                    {
                        if ((cell.StringCellValue.Trim().ToUpper() ?? "") == (header ?? ""))
                            return cell.ColumnIndex;
                    }
            }
            return default;
        }
        private static DateTime FindPayrollDate(ISheet nSheet)
        {
            string payrollDateRaw = "";

            if (nSheet.GetRow(3) is not null && nSheet.GetRow(3).GetCell(1) is not null)
                payrollDateRaw = nSheet.GetRow(3).GetCell(1).StringCellValue.Trim().Replace("*", "").Trim();
            else if (nSheet.GetRow(4) is not null && nSheet.GetRow(4).GetCell(0) is not null)
                payrollDateRaw = nSheet.GetRow(4).GetCell(0).StringCellValue.Split(':')[1].Trim();

            return DateTime.ParseExact(payrollDateRaw, "dd MMMM yyyy", CultureInfo.InvariantCulture);
        }
        private static string[] ParseEmployeeDetail(IRow row, int nameIdx)
        {
            if (row.GetCell(nameIdx) is not null)
            {
                var fullname_raw = row.GetCell(nameIdx).StringCellValue.Trim(')').Split('(');
                if (fullname_raw.Length < 2)
                    return null;

                return new[] { fullname_raw[0].Trim(), fullname_raw[1].Trim() };
            }
            return null;
        }
    }
}
