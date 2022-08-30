using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pms.Payrolls.Domain.SupportTypes
{
    public class AlphalistDetail
    {
        public string RegisteredName;
        public string SequenceNumber;
        public string ScheduleNumber;
        public string ReturnPeriod;
        public string EmployerBranchCode;
        public string EmployerTin;
        public string FormType;
        public string ExmpnCode;
        public string SubsFiling;
        public string RegionNumber;
        public string StatusCode;
        public string AtcCode;
        public string BranchCode;
        public double IncomePayment;
        public double PreviousTaxableSalary;
        public double PreviousTaxable13thMonth;
        public double PreviousTaxWithheld;
        public double PreviousNonTaxableSalary;
        public double PreviousNonTaxable13thMonth;
        public double PreviousNonTaxableSssGsisOtherContribution;
        public double TaxRate;
        public double ExmpnAmount;
        public double HeathPremium;
        public double FringeBenefit;
        public double MonetaryValue;
        public double PreviousNonTaxableDeMinimis;
        public double PreviousTotalNonTaxableCompensationIncome;
        public double PreviousTaxableBasicSalary;
        public double PresentTaxableBasicSalary;
        public double PreviousAndPresentTotalTaxable;
        public double PreviousNonTaxableHazardPay;
        public double PreviousNonTaxableNightDifferential;
        public double PreviousNonTaxableOvertimePay;
        public double PreviousNonTaxableHolidayPay;
        public double PreviousNonTaxableBasicSmw;
        public double PreviousAndPresentTotalCompensationIncome;
        public double TotalNontaxableCompensationIncome;
        public double TotalTaxableCompensationIncome;
        public double PreviousTotalTaxable;
        public int QrtNumber;
        public string Address1;
        public string Address2;
        public string AtcDescription;

        public string EEId { get; set; }

        public string Company { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string MiddleName { get; set; }

        public string Tin { get; set; }

        public DateTime StartDate { get; set; }
        
        public DateTime ResignationDate { get; set; }

        public double AcutalAmountWithheld { get; set; }

        public int FactorUsed { get; set; } = 313;

        public double PresentTaxableSalary { get; set; }

        public double PresentTaxable13thMonth { get; set; }

        public double PresentTaxWithheld { get; set; }

        public double PresentNonTaxableSalary { get; set; }

        public double PresentNonTaxable13thMonth { get; set; }

        public double PresentNonTaxableSssGsisOtherContribution { get; set; }

        public double OverWithheld { get; set; }

        public double AmmountWithheldOnDecember { get; set; }

        public double TaxDue { get; set; }

        public double NetTaxableCompensationIncome { get; set; }

        public double GrossCompensationIncome { get; set; }

        public double PresentNonTaxableDeMinimis { get; set; } = 0;

        public double PresentTotalCompensation { get; set; }

        public double PresentTotalNonTaxableCompensationIncome { get; set; }

        public double PresentNonTaxableGrossCompensationIncome { get; set; }

        public double PresentNonTaxableBasicSmwHour { get; set; }

        public double PresentNonTaxableBasicSmwDay { get; set; }

        public double PresentNonTaxableBasicSmwMonth { get; set; }
        
        public double PresentNonTaxableBasicSmwYear { get; set; }

        public double PresentNonTaxableHolidayPay { get; set; }

        public double PresentNonTaxableOvertimePay { get; set; }

        public double PresentNonTaxableNightDifferential { get; set; }

        public double PresentNonTaxableHazardPay { get; set; } = 0;

        public double NonTaxableBasicSalary { get; set; }
        
        public double TaxableBasicSalary { get; set; }

        public string Nationality { get; set; }
        
        public string ReasonForSeparation { get; set; }

        public string EmploymentStatus{ get; set; }

        public double December { get; set; }

        public double Final { get; set; }
    }
}
