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
        /// <summary>
        /// Used for extracting banks
        /// </summary>
        /// <returns></returns>
        IEnumerable<Payroll> GetAllPayrolls();


        IEnumerable<Payroll> GetPayrolls(string cutoffId);

        IEnumerable<Payroll> GetPayrolls(string cutoffId, string payrollCode);

        IEnumerable<Payroll> GetPayrolls(int yearsCovered, string companyId);


        /// <summary>
        /// Used for generating Government Computation.
        /// </summary>
        /// <param name="month"></param>
        /// <param name="payrollCode"></param>
        /// <returns></returns>
        IEnumerable<Payroll> GetMonthlyPayrolls(int month, string payrollCode);
        
        IEnumerable<Payroll> GetNoEEPayrolls();
    }
}
