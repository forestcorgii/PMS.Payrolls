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
        public string PayrollId { get; set; }

        public virtual TimesheetView TS { get; set; }

        public string EEId { get; set; }
        public virtual EmployeeView EE { get; set; }

        public string CutoffId { get; set; }
        public Cutoff Cutoff { get => new Cutoff(CutoffId); }

        public string CompanyId { get; set; }
        //public Company Company { get; set; }

        public string PayrollCode { get; set; }
        public string BankCategory { get; set; }
        public BankChoices Bank { get; set; }


        public double RegHours { get; set; }
        public double Overtime { get; set; }
        public double RestDayOvertime { get; set; }
        public double HolidayOvertime { get; set; }
        public double NightDifferential { get; set; }
        public double AbsTar { get; set; }// Absent & Tardy

        public double GrossPay { get; set; }
        public double RegPay { get; set; }
        public double NetPay { get; set; }

        public double EmployeeSSS { get; set; }
        public double EmployeePhilHealth { get; set; }
        public double EmployeePagibig { get; set; }

        public double WithholdingTax { get; set; }

        public double Adjust1Total { get; set; }
        public double Adjust2Total { get; set; }

        public double GovernmentTotal { get; set; }

        public int YearCovered { get; set; }


        public static string GenerateId(Payroll payroll) => $"{payroll.EEId}_{payroll.CutoffId}";

        public double Rate
        {
            get
            {
                if (RegHours > 96)
                {
                    double AdjustedRegHours = RegHours - AbsTar;
                    return RegPay / AdjustedRegHours;
                }
                return RegPay / RegHours;
            }
        }
        public double AdjustedRegPay
        {
            get
            {
                if (RegHours > 96)
                {
                    return Rate * 96;
                }
                return RegPay;
            }
        }
    }
}
