using NPOI.SS.UserModel;
using Pms.Payrolls.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pms.Payrolls.ServiceLayer.Files.Exports.Governments.Macros
{
    public class WithholdingTaxRowWriter : IRowWriter
    {
        public void Write(IRow row, Payroll payroll, int sequence)
        {
            row.CreateCell(0).SetCellValue(sequence);
            row.CreateCell(1).SetCellValue(payroll.EEId);
            row.CreateCell(2).SetCellValue(payroll.EE.Fullname);
            row.CreateCell(3).SetCellValue(payroll.WithholdingTax);
        }

        public void WriteTotal(IRow row, IEnumerable<Payroll> payrolls)
        {
            row.CreateCell(2).SetCellValue($"TOTAL");
            row.CreateCell(3).SetCellValue(payrolls.Sum(p => p.WithholdingTax));
        }
    }
}
