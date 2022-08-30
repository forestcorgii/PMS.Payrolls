using DotNetDBF;
using Pms.Payrolls.Domain.SupportTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pms.Payrolls.ServiceLayer.Files
{
    public class AlphalistImport
    {
        public void ImportToBIRProgram()
        {

        }

        private void WriteRecordToDbf(DBFWriter writer,AlphalistDetail alpha)
        {
            string[] alphaRecord = new[]
            {
                alpha.FormType,
                alpha.EmployerTin,
                alpha.EmployerBranchCode,
                alpha.ReturnPeriod,
                alpha.ScheduleNumber,
                alpha.SequenceNumber,
                alpha.RegisteredName,
                alpha.EEId,
                alpha.FirstName,
                alpha.LastName,
                alpha.MiddleName,
                alpha.Tin,
                alpha.BranchCode,
                alpha.StartDate.ToString("dd/MM/yyyy"),
                alpha.ResignationDate.ToString("dd/MM/yyyy"),
                alpha.AtcCode,
                alpha.StatusCode,
                alpha.RegionNumber,
                alpha.SubsFiling,
                alpha.ExmpnCode,
                alpha.FactorUsed.ToString("##.##"),
                alpha.AcutalAmountWithheld.ToString("##.##"),
                alpha.IncomePayment.ToString("##.##"),
                alpha.PresentTaxableSalary.ToString("##.##"),
                alpha.PresentTaxable13thMonth.ToString("##.##"),
                alpha.PresentTaxWithheld.ToString("##.##"),
                alpha.PresentNonTaxableSalary.ToString("##.##"),
                alpha.PresentNonTaxable13thMonth.ToString("##.##"),
                alpha.PreviousTaxableSalary.ToString("##.##"),
                alpha.PreviousTaxable13thMonth.ToString("##.##"),
                alpha.PreviousTaxWithheld.ToString("##.##"),
                alpha.PreviousNonTaxableSalary.ToString("##.##"),
                alpha.PreviousNonTaxable13thMonth.ToString("##.##"),
                alpha.PresentNonTaxableSssGsisOtherContribution.ToString("##.##"),
                alpha.PreviousNonTaxableSssGsisOtherContribution.ToString("##.##"),
                alpha.TaxRate.ToString("##.##"),
                alpha.OverWithheld.ToString("##.##"),
                alpha.AmmountWithheldOnDecember.ToString("##.##"),
                alpha.ExmpnAmount.ToString("##.##"),
                alpha.TaxDue.ToString("##.##"),
                alpha.HeathPremium.ToString("##.##"),
                alpha.FringeBenefit.ToString("##.##"),
                alpha.MonetaryValue.ToString("##.##"),
                alpha.NetTaxableCompensationIncome.ToString("##.##"),
                alpha.GrossCompensationIncome.ToString("##.##"),
                alpha.PreviousNonTaxableDeMinimis.ToString("##.##"),
                alpha.PreviousTotalNonTaxableCompensationIncome.ToString("##.##"),
                alpha.PreviousTaxableBasicSalary.ToString("##.##"),
                alpha.PresentNonTaxableDeMinimis.ToString("##.##"),
                alpha.PresentTaxableBasicSalary.ToString("##.##"),
                alpha.PresentTotalCompensation.ToString("##.##"),
                alpha.PreviousAndPresentTotalTaxable.ToString("##.##"),
                alpha.PresentTotalNonTaxableCompensationIncome.ToString("##.##"),
                alpha.PreviousNonTaxableBasicSmw.ToString("##.##"),
                alpha.PreviousNonTaxableHolidayPay.ToString("##.##"),
                alpha.PreviousNonTaxableOvertimePay.ToString("##.##"),
                alpha.PreviousNonTaxableNightDifferential.ToString("##.##"),
                alpha.PreviousNonTaxableHazardPay.ToString("##.##"),

                alpha.PresentNonTaxableGrossCompensationIncome.ToString("##.##"),
                alpha.PresentNonTaxableBasicSmwDay.ToString("##.##"),
                alpha.PresentNonTaxableBasicSmwMonth.ToString("##.##"),
                alpha.PresentNonTaxableBasicSmwYear.ToString("##.##"),
                alpha.PresentNonTaxableHolidayPay.ToString("##.##"),
                alpha.PresentNonTaxableOvertimePay.ToString("##.##"),
                alpha.PresentNonTaxableNightDifferential.ToString("##.##"),
                alpha.PreviousAndPresentTotalCompensationIncome.ToString("##.##"),
                alpha.PresentNonTaxableHazardPay.ToString("##.##"),
                alpha.TotalNontaxableCompensationIncome.ToString("##.##"),
                alpha.TotalTaxableCompensationIncome.ToString("##.##"),
                alpha.PreviousTotalTaxable.ToString("##.##"),
                alpha.NonTaxableBasicSalary.ToString("##.##"),
                alpha.TaxableBasicSalary.ToString("##.##"),
                alpha.QrtNumber.ToString(),
                "{//}",
                alpha.Nationality,
                alpha.ReasonForSeparation,
                alpha.EmploymentStatus,
                alpha.Address1,
                alpha.Address2,
                alpha.AtcDescription,
                "{//}",
                "{//}",

            };

            writer.AddRecord(alphaRecord);
        }
    }
}
