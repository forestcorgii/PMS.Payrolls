using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pms.Payrolls.Domain.SupportTypes
{
    public class AlphalistDetail
    {
        public string FormType = string.Empty;

        public string EmployerTin = string.Empty;

        public string EmployerBranchCode = string.Empty;

        public DateTime ReturnPeriod;

        public string ScheduleNumber = string.Empty;

        public double SequenceNumber = 0;

        public string RegisteredName = string.Empty;

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string MiddleName { get; set; } = string.Empty;

        public string Tin { get; set; } = string.Empty;

        public string BranchCode = string.Empty;

        public DateTime StartDate { get; set; }

        public DateTime ResignationDate { get; set; }

        public string AtcCode = string.Empty;

        public string StatusCode = string.Empty;

        public string RegionNumber = string.Empty;

        public string SubsFiling = string.Empty;

        public string ExmpnCode = string.Empty;

        public double FactorUsed { get; set; } = 313;

        public double AcutalAmountWithheld { get; set; }

        public double IncomePayment = 0;

        public double PresentTaxableSalary { get; set; }

        public double PresentTaxable13thMonth { get; set; }

        public double PresentTaxWithheld { get; set; }

        public double PresentNonTaxableSalary { get; set; }

        public double PresentNonTaxable13thMonth { get; set; }

        public double PreviousTaxableSalary = 0;

        public double PreviousTaxable13thMonth = 0;

        public double PreviousTaxWithheld = 0;

        public double PreviousNonTaxableSalary = 0;

        public double PreviousNonTaxable13thMonth = 0;

        public double PresentNonTaxableSssGsisOtherContribution { get; set; }

        public double PreviousNonTaxableSssGsisOtherContribution = 0;

        public double TaxRate = 0;

        public double OverWithheld { get; set; }

        public double AmmountWithheldOnDecember { get; set; }

        public double ExmpnAmount = 0;

        public double TaxDue { get; set; }

        public double HeathPremium = 0;

        public double FringeBenefit = 0;

        public double MonetaryValue = 0;

        public double NetTaxableCompensationIncome { get; set; }

        public double GrossCompensationIncome { get; set; }

        public double PreviousNonTaxableDeMinimis = 0;

        public double PreviousTotalNonTaxableCompensationIncome = 0;

        public double PreviousTaxableBasicSalary = 0;

        public double PresentNonTaxableDeMinimis { get; set; } = 0;

        public double PresentTaxableBasicSalary = 0;

        public double PresentTotalCompensation { get; set; }

        public double PreviousAndPresentTotalTaxable = 0;

        public double PresentTotalNonTaxableCompensationIncome { get; set; }

        public double PreviousNonTaxableGrossCompensationIncome { get; set; }

        public double PreviousNonTaxableBasicSmw = 0;

        public double PreviousNonTaxableHolidayPay = 0;

        public double PreviousNonTaxableOvertimePay = 0;

        public double PreviousNonTaxableNightDifferential = 0;

        public double PreviousNonTaxableHazardPay = 0;

        public double PresentNonTaxableGrossCompensationIncome { get; set; }

        public double PresentNonTaxableBasicSmwDay { get; set; } = 0;

        public double PresentNonTaxableBasicSmwMonth { get; set; } = 0;

        public double PresentNonTaxableBasicSmwYear { get; set; } = 0;

        public double PresentNonTaxableHolidayPay { get; set; } = 0;

        public double PresentNonTaxableOvertimePay { get; set; } = 0;

        public double PresentNonTaxableNightDifferential { get; set; }

        public double PreviousAndPresentTotalCompensationIncome = 0;

        public double PresentNonTaxableHazardPay { get; set; } = 0;

        public double TotalNontaxableCompensationIncome = 0;

        public double TotalTaxableCompensationIncome = 0;

        public double PreviousTotalTaxable = 0;

        public double NonTaxableBasicSalary { get; set; }

        public double TaxableBasicSalary { get; set; }

        public int QrtNumber = 0;

        public DateTime QuarterDate;

        public string Nationality { get; set; }

        public string ReasonForSeparation { get; set; } = string.Empty;

        public string EmploymentStatus { get; set; } = string.Empty;

        public string Address1 = string.Empty;

        public string Address2 = string.Empty;

        public string AtcDescription = string.Empty;
        
        public DateTime DateOfDeath;
        
        public DateTime DateWithheld;



        public string EEId { get; set; } = string.Empty;

        //public string Company { get; set; } = string.Empty;

        public double PresentNonTaxableBasicSmwHour { get; set; }

        public double December { get; set; }

        public double Final { get; set; }
    }
}
