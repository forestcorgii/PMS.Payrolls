using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pms.Payrolls.Domain.SupportTypes
{
    public class ThirteenthMonth
    {
        public string EEId { get; set; }
        public string Fullname { get; set; }
        public double TotalRegPay { get; set; }
        public double Amount { get; set; }
        public double TotalAmountDeducted { get; set; }
    }
}
