using NPOI.SS.UserModel;
using Pms.Payrolls.Domain;
using Pms.Payrolls.Domain.SupportTypes;
using System.Collections.Generic;
using System.Linq;

namespace Pms.Payrolls.ServiceLayer.Files.Exports.Governments.Macros
{
    public class PagibigRowWriter : IRowWriter
    {
        public void Write(IRow row, Payroll payroll, int sequence)
        {
            row.CreateCell(0).SetCellValue(sequence);
            row.CreateCell(1).SetCellValue(payroll.EEId);
            row.CreateCell(2).SetCellValue(payroll.EE.Fullname);
            row.CreateCell(3).SetCellValue(payroll.EmployeePagibig);
            row.CreateCell(4).SetCellValue(payroll.EmployeePagibig);// should be employer
            row.CreateCell(5).SetCellValue(payroll.EmployeePagibig + payroll.EmployeePagibig);// should be employer
        }

        public void WriteTotal(IRow row, PayrollRegister payrollRegister)
        {
            
            row.CreateCell(2).SetCellValue($"{payrollRegister.Name} TOTAL"); 
            row.CreateCell(3).SetCellValue(payrollRegister.EmployeePhilHealth);
            row.CreateCell(4).SetCellValue(payrollRegister.EmployeePhilHealth);// should be employer
            row.CreateCell(5).SetCellValue(payrollRegister.EmployeePhilHealth + payrollRegister.EmployeePhilHealth);// should be employer
             
        }
    }
}
