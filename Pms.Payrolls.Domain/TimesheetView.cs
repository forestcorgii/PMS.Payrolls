using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pms.Payrolls.Domain
{
    public class TimesheetView
    {
        public string CutoffId { get; set; }
        public string TimesheetId { get; set; }
        public string EEId { get; set; }

        public string PayrollCode { get; private set; }
        public string BankCategory { get; private set; }

        public double TotalHours { get; set; }

        public double TotalOT { get; set; }

        public double TotalRDOT { get; set; }

        public double TotalHOT { get; set; }

        public double TotalND { get; set; }

        public double TotalTardy { get; set; }










    }
}
