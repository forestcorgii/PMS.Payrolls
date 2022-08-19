using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Pms.Payrolls.Domain.Enums;

namespace Pms.Payrolls.Domain
{
    public class Payroll
    {
        public string EEId { get; set; }
        public virtual EmployeeView EE { get; set; }

        public string PayrollCode { get; set; }
        public string BankCategory { get; set; }
        public BankType Bank { get; set; }

        public string PayrollId { get; set; }
        public string CutoffId { get; set; }

        public double RegHours { get; set; }
        public double AbsTar { get; set; }// Absent & Tardy

        public double GrossPay { get; set; }
        public double RegPay { get; set; }
        public double NetPay { get; set; }

        public double Adjust1Total { get; set; }
        public double Adjust2Total { get; set; }
        public double GovernmentTotal { get; set; }
        public int YearCovered { get; set; }


        public static string GenerateId(Payroll payroll) => $"{payroll.EEId}_{payroll.CutoffId}";

        public double AdjustedRegPay()
        {
            if (RegHours > 96)
            {
                double AdjustedRegHours = RegHours - AbsTar;
                double hourlyRate = RegPay / AdjustedRegHours;

                return hourlyRate * 96;
            }
            return RegPay;
        }
    }
}
