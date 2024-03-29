﻿using System;
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

        public string EEId { get; set; }
        public virtual EmployeeView EE { get; set; }

        public string CutoffId { get; set; }
        public Cutoff Cutoff { get => new Cutoff(CutoffId); }

        public string CompanyId { get; set; }
        //public Company Company { get; set; }

        public string PayrollCode { get; set; }

        public double RegHours { get; set; }

        public double Overtime { get; set; }
        public double OvertimeAmount { get => Overtime * Rate * 1.25; }

        public double RestDayOvertime { get; set; }
        public double RestDayOvertimeAmount { get => RestDayOvertime * Rate * 1.3; }

        public double HolidayOvertime { get; set; }
        public double HolidayOvertimeAmount { get => HolidayOvertime * Rate * 2.0; }

        public double NightDifferential { get; set; }
        public double NightDifferentialAmount { get => NightDifferential * Rate * 0.1; }


        public double AbsTar { get; set; }// Absent & Tardy

        public double GrossPay { get; set; }
        public double RegularPay { get; set; }
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
                if (RegHours > 0 && RegularPay > 0)
                {
                    double AdjustedRegHours = RegHours - AbsTar;
                    return RegularPay / AdjustedRegHours;
                }
                return 0;
            }
        }

        public double OriginalRate
        {
            get
            {
                return RegularPay / RegHours;
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
                return RegularPay;
            }
        }


        public bool IsReadyForExport() => EE is null || EE.AccountNumber == "";

        public void UpdateValues()
        {
            EmployeeSSS = ConvertToPositive(EmployeeSSS);
            EmployeePagibig = ConvertToPositive(EmployeePagibig);
            EmployeePhilHealth = ConvertToPositive(EmployeePhilHealth);
        }

        private double ConvertToPositive(double value)
        {
            if (value < 0)//ensure negative value    
                return value - (value * 2);
            return value;
        }
    }
}
