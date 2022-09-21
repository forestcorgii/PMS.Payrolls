using NPOI.SS.UserModel;
using Pms.Payrolls.Domain;
using System.Collections.Generic;
using System.Linq;

namespace Pms.Payrolls.ServiceLayer.Files.Exports.Governments
{
    public class RegularSheetWriter : ISheetWriter
    {
        public IRowWriter RowWriter;
        readonly int StartIndex;
        readonly IEnumerable<Payroll> Payrolls;

        public RegularSheetWriter(IEnumerable<Payroll> payrolls, IRowWriter rowWriter, int startIndex = 1)
        {
            StartIndex = startIndex;
            RowWriter = rowWriter;
            Payrolls = payrolls.OrderBy(p => p.EE.Fullname);
        }

        public RegularSheetWriter(IEnumerable<Payroll> payrolls, int startIndex = 1)
        {
            StartIndex = startIndex;
            Payrolls = payrolls.OrderBy(p => p.EE.Fullname);
        }

        public void Write(ISheet sheet)
        {
            int index = StartIndex;
            int sequence = 0;
            foreach (Payroll payroll in Payrolls)
                RowWriter.Write(sheet.CreateRow(append(ref index)), payroll, append(ref sequence));

            append(ref index);
            append(ref index);
            IRow rowForTotal = sheet.CreateRow(append(ref index));
            RowWriter.WriteTotal(rowForTotal, Payrolls);
        }


        private static int append(ref int index)
        {
            index++;
            return index;
        }
    }
}
