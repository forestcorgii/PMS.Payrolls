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
        IEnumerable<Payroll> GetPayrolls(string cutoffId);
        IEnumerable<Payroll> GetPayrolls(string cutoffId, BankChoices bankType);
        IEnumerable<Payroll> GetPayrolls(int yearsCovered, BankChoices bankType);
        IEnumerable<Payroll> GetPayrolls(int yearsCovered, string companyId);
        IEnumerable<Payroll> GetPayrolls(string cutoffId, string payrollCode, BankChoices bankType);
        IEnumerable<Payroll> GetAllPayrolls();
        IEnumerable<Company> GetAllCompanies();
        IEnumerable<Payroll> GetNoEEPayrolls();
    }
}
