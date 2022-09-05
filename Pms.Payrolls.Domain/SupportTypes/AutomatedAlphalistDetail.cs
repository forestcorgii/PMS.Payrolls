﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pms.Payrolls.Domain.SupportTypes
{
    public class AutomatedAlphalistDetail : IAlphalistDetail
    {
        #region EMPLOYEE INFORMATION
        private string EEId;
        private string Company;
        private string FirstName;
        private string LastName;
        private string MiddleName;
        private string TIN;

        private string StartDate;
        private string ResignationDate;
        private string Nationality { get; set; }
        private string ReasonForSeparation { get; set; }
        private string EmploymentStatus { get; set; }
        #endregion

        private int FactorUsed { get; set; } = 313;

        private double PresentTaxableSalary;

        private double PresentTaxable13thMonth;

        #region PerDay-specific Properties
        private double PresentNonTaxableBasicSmwHour = 0;
        private double PresentNonTaxableBasicSmwDay => PresentNonTaxableBasicSmwHour * 8;
        private double PresentNonTaxableBasicSmwMonth => int.Parse(PresentNonTaxableBasicSmwDay.ToString("0.00").Split(".")[0]) * 24;
        private double PresentNonTaxableBasicSmwYear => PresentNonTaxableBasicSmwMonth * 12;

        private double Overtime;
        private double OvertimeAmount => Overtime * PresentNonTaxableBasicSmwHour * 1.25;
        private double RestDayOvertime;
        private double RestDayOvertimeAmount => RestDayOvertime * PresentNonTaxableBasicSmwHour * 1.3;

        private double HolidayOvertime;
        private double PresentNonTaxableHolidayPay => HolidayOvertime * PresentNonTaxableBasicSmwHour * 2.0;

        private double NightDifferential;
        private double PresentNonTaxableNightDifferential => NightDifferential * PresentNonTaxableBasicSmwHour * 0.1;

        private double PresentNonTaxableOvertimePay => OvertimeAmount + RestDayOvertimeAmount;
        #endregion

        #region PAY
        private double RegularPay;
        private double GrossPay;
        private double NetPay;

        private double Taxable13thMonth
        {
            get
            {
                //if (NonTaxable13thMonth > 250000)
                //    return NonTaxable13thMonth - 250000;
                return 0;
            }
        }
        private double PresentNonTaxable13thMonth;

        private double NonTaxableSalary
        {
            get
            {
                double nonTaxableSalary = GrossPay - (PresentNonTaxable13thMonth + PresentNonTaxableSssGsisOtherContribution);

                if (PresentNonTaxableBasicSmwHour < 70.2)
                    nonTaxableSalary -= (PresentNonTaxableNightDifferential + OvertimeAmount + RestDayOvertimeAmount + PresentNonTaxableHolidayPay);

                return nonTaxableSalary;
            }
        }

        private double PresentNonTaxableDeMinimis { get; set; } = 0;

        private double GrossCompensationIncome => NonTaxableSalary > 250000 ? NonTaxableSalary : 0;
        private double PresentNonTaxableSalary => GrossCompensationIncome;
        private double NetTaxableCompensationIncome => GrossCompensationIncome;
        private double PresentTotalCompensation => GrossCompensationIncome;

        private double TaxableBasicSalary => NonTaxableSalary > 250000 ? NonTaxableSalary : 0;

        private double PresentTotalNonTaxableCompensationIncome => NonTaxableSalary + PresentNonTaxable13thMonth + PresentNonTaxableSssGsisOtherContribution;
        private double PresentNonTaxableGrossCompensationIncome => NonTaxableSalary + PresentNonTaxable13thMonth + PresentNonTaxableSssGsisOtherContribution;
        #endregion

        #region PresentNonTaxableSssGsisOtherContribution
        private double EmployeeSSS;
        private double EmployeePhilHealth;
        private double EmployeePagibig;
        private double PresentNonTaxableSssGsisOtherContribution => EmployeeSSS + EmployeePhilHealth + EmployeePagibig;
        #endregion

        #region TAX
        private double WithholdingTax = 0;
        private double DecemberTaxWithheld = 0;
        private double PresentTaxWithheld => WithholdingTax - DecemberTaxWithheld;

        private double TaxDue
        {
            get
            {
                double taxableSalary = NonTaxableSalary;
                switch (taxableSalary)
                {
                    case var case3 when case3 >= 8000000:
                        return 2410000 + (taxableSalary - 8000000) * 0.35;
                    case var case4 when case4 >= 2000000:
                        return 490000 + (taxableSalary - 2000000) * 0.32;
                    case var case5 when case5 >= 800000:
                        return 130000 + (taxableSalary - 800000) * 0.3;
                    case var case6 when case6 >= 400000:
                        return 30000 + (taxableSalary - 400000) * 0.25;
                    case var case7 when case7 >= 250000:
                        return 0 + (taxableSalary - 250000) * 0.2;
                    case var case8 when case8 <= 250000:
                        return 0;
                }
                return 0;
            }
        }

        private double AmmountWithheldOnDecember
        {
            get
            {
                if (TaxDue > PresentTaxWithheld)
                    return TaxDue - PresentTaxWithheld;
                return 0;
            }
        }

        private double OverWithheld
        {
            get
            {
                if (TaxDue < PresentTaxWithheld)
                    return PresentTaxWithheld - TaxDue;
                return 0;
            }
        }

        private double AcutalAmountWithheld
        {
            get
            {
                if (TaxDue > PresentTaxWithheld)
                    return PresentTaxWithheld + AmmountWithheldOnDecember;
                else
                    return PresentTaxWithheld - OverWithheld;
            }
        }

        private double NonTaxableBasicSalary { get; set; }
        private double PresentNonTaxableHazardPay { get; set; }

        #endregion

        public AutomatedAlphalistDetail(IEnumerable<Payroll> yearlyPayrolls, double minimumRate)
        {
            Payroll payroll = yearlyPayrolls.First();
            PresentNonTaxableBasicSmwHour = payroll.Rate > minimumRate ? payroll.Rate : minimumRate;

            EmployeeView ee = payroll.EE;
            EEId = ee.EEId;
            FirstName = ee.FirstName;
            LastName = ee.LastName;
            MiddleName = ee.MiddleName;
            TIN = ee.TIN;
            Nationality = "FILIPINO";
            EmploymentStatus = "R";
            ReasonForSeparation = "";

            StartDate = yearlyPayrolls.First().Cutoff.CutoffDate.ToString("dd/MM/yyyy");
            ResignationDate = yearlyPayrolls.Last().Cutoff.CutoffDate.ToString("dd/MM/yyyy");


            Overtime = yearlyPayrolls.Sum(py => py.Overtime);
            RestDayOvertime = yearlyPayrolls.Sum(py => py.RestDayOvertime);
            HolidayOvertime = yearlyPayrolls.Sum(py => py.HolidayOvertime);
            NightDifferential = yearlyPayrolls.Sum(py => py.NightDifferential);

            RegularPay = yearlyPayrolls.Sum(py => py.RegularPay);
            GrossPay = yearlyPayrolls.Sum(py => py.GrossPay);
            NetPay = yearlyPayrolls.Sum(py => py.NetPay);

            EmployeeSSS = yearlyPayrolls.Sum(py => py.EmployeeSSS);
            EmployeePagibig = yearlyPayrolls.Sum(py => py.EmployeePagibig);
            EmployeePhilHealth = yearlyPayrolls.Sum(py => py.EmployeePhilHealth);

            WithholdingTax = yearlyPayrolls.Sum(py => py.WithholdingTax);
            DecemberTaxWithheld = yearlyPayrolls
                .Where(py => py.Cutoff.CutoffDate.Month == 12)
                .Sum(py => py.WithholdingTax);

            PresentNonTaxable13thMonth = yearlyPayrolls.Sum(py => py.AdjustedRegPay) / 12;
        }

        public AlphalistDetail CreateAlphalistDetail()
        {
            AlphalistDetail a = new();
            a.EEId = EEId;
            //a.Company = Company;
            a.FirstName = FirstName;
            a.LastName = LastName;
            a.MiddleName = MiddleName;
            a.Tin = TIN;
            a.StartDate =DateTime.Parse( StartDate);
            a.ResignationDate = DateTime.Parse(ResignationDate);
            a.AcutalAmountWithheld = AcutalAmountWithheld;
            a.FactorUsed = FactorUsed;
            a.PresentTaxableSalary = PresentTaxableSalary;
            a.PresentTaxable13thMonth = PresentTaxable13thMonth;
            a.PresentTaxWithheld = PresentTaxWithheld;
            a.PresentNonTaxableSalary = PresentNonTaxableSalary;
            a.PresentNonTaxable13thMonth = PresentNonTaxable13thMonth;
            a.PresentNonTaxableSssGsisOtherContribution = PresentNonTaxableSssGsisOtherContribution;
            a.OverWithheld = OverWithheld;
            a.AmmountWithheldOnDecember = AmmountWithheldOnDecember;
            a.TaxDue = TaxDue;
            a.NetTaxableCompensationIncome = NetTaxableCompensationIncome;
            a.GrossCompensationIncome = GrossCompensationIncome;
            a.PresentNonTaxableDeMinimis = PresentNonTaxableDeMinimis;
            a.PresentTotalCompensation = PresentTotalCompensation;
            a.PresentTotalNonTaxableCompensationIncome = PresentTotalNonTaxableCompensationIncome;
            a.PresentNonTaxableGrossCompensationIncome = PresentNonTaxableGrossCompensationIncome;
            a.PresentNonTaxableBasicSmwHour = PresentNonTaxableBasicSmwHour;
            a.PresentNonTaxableBasicSmwDay = PresentNonTaxableBasicSmwDay;
            a.PresentNonTaxableBasicSmwMonth = PresentNonTaxableBasicSmwMonth;
            a.PresentNonTaxableBasicSmwYear = PresentNonTaxableBasicSmwYear;
            a.PresentNonTaxableHolidayPay = PresentNonTaxableHolidayPay;
            a.PresentNonTaxableOvertimePay = PresentNonTaxableOvertimePay;
            a.PresentNonTaxableNightDifferential = PresentNonTaxableNightDifferential;
            a.PresentNonTaxableHazardPay = PresentNonTaxableHazardPay;
            a.NonTaxableBasicSalary = NonTaxableBasicSalary;
            a.TaxableBasicSalary = TaxableBasicSalary;
            a.Nationality = Nationality;
            a.ReasonForSeparation = ReasonForSeparation;
            a.EmploymentStatus = EmploymentStatus;
            a.December = DecemberTaxWithheld;
            a.Final = OverWithheld;

            return a;
        }
    }
}