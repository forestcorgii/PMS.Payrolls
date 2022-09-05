using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pms.Payrolls.Domain
{
    public static class Enums
    {
        /// <summary>
        /// CHK - Check
        /// LBP - Land Bank of the Philippines
        /// CHB - China Bank
        /// MTAC - Metro Bank Tacloban
        /// MPALO - Metro Bank Palo
        /// </summary>
        public enum BankChoices
        {
            CHK = 0000,
            LBP = 1000,
            CBC = 1001,
            MTAC = 1002,
            MPALO = 1003,
        }

        public enum ImportProcessChoices
        {
            PD = 10,
            KS = 11
        }

        public enum SiteChoices
        {
            MANILA = 1415,
            LEYTE = 1642,
        }

        public enum AlphalistScheduleNumberChoices
        {
            D1 = 1232,
            D2 = 1351,
        }

    }
}
