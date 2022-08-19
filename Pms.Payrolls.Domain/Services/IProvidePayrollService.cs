using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Pms.Payrolls.Domain.Enums;

namespace Pms.Payrolls.Domain.Services
{
    public interface IProvidePayrollService
    {
        IEnumerable<Payroll> GetPayrolls(string cutoffId,BankType bankType);
        IEnumerable<Payroll> GetPayrolls(int yearsCovered, BankType bankType);
        IEnumerable<Payroll> GetAllPayrolls();
    }
}
