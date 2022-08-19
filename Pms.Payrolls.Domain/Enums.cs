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
        /// LBP - Land Bank of the Philippines
        /// CHB - China Bank
        /// </summary>
        public enum BankType
        {
            LBP = 1000,
            CHB = 1001,
            MTAC = 1001,
            MPALO = 1001,
            CHK = 1001,
        }
    }
}
