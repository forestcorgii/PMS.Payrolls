using ExcelDataReader;
using Pms.Payrolls.Domain;
using Pms.Payrolls.Domain.Exceptions;
using Pms.Payrolls.Domain.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Pms.Payrolls.ServiceLayer.Files
{
    public class PayrollRegisterKSImport : IImportPayrollService
    {
        private int NameIndex = 1;

        private int RegularHoursIndex = 2;

        private int GrossPayIndex = 5;

        private int NightDifferentialIndex = 6;

        private int EmployeePagibigIndex = 7;
        private int EmployerPagibigIndex = 8;

        private int EmployeeSSSIndex = 9;
        private int EmployerSSSIndex = 10;

        private int EmployeePhilHealthIndex = 11;

        private int WithholdingTaxIndex = 12;

        private int NetpayIndex = 15;

        private string PayrollRegisterFilePath;

        DateTime CutoffDate { get; set; }

        public void ValidatePayRegisterFile()
        {

            if (CutoffDate == default) throw new PayrollRegisterHeaderNotFoundException("Cutoff Date", PayrollRegisterFilePath);
        }


        public IEnumerable<Payroll> StartImport(string payrollRegisterFilePath)
        {
            PayrollRegisterFilePath = payrollRegisterFilePath;

            CutoffDate = default;
            List<Payroll> payrolls = new();
            using (var stream = File.Open(payrollRegisterFilePath, FileMode.Open, FileAccess.Read))
            {
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    //FindHeaders(reader);
                    FindCutoffDate(reader);

                    ValidatePayRegisterFile();

                    Cutoff cutoff = new Cutoff(CutoffDate);
                    do
                    {
                        string eeId = "";
                        var name_args = ParseEmployeeDetail(reader, NameIndex);
                        if (name_args is null)
                            continue;
                        eeId = name_args[1].Trim();

                        var newPayroll = new Payroll()
                        {
                            EEId = eeId,
                            CutoffId = cutoff.CutoffId,
                            YearCovered = cutoff.YearCovered,
                        };

                        newPayroll.RegHours = reader.GetDouble(RegularHoursIndex);

                        newPayroll.RegularPay = reader.GetDouble(GrossPayIndex);
                        newPayroll.GrossPay = reader.GetDouble(GrossPayIndex);

                        newPayroll.NightDifferential = reader.GetDouble(NightDifferentialIndex);

                        newPayroll.EmployeePagibig = reader.GetDouble(EmployeePagibigIndex);
                        newPayroll.EmployerPagibig = reader.GetDouble(EmployerPagibigIndex);

                        newPayroll.EmployeeSSS = reader.GetDouble(EmployeeSSSIndex);
                        newPayroll.EmployerSSS = reader.GetDouble(EmployerSSSIndex);

                        newPayroll.EmployeePhilHealth = reader.GetDouble(EmployeePhilHealthIndex);

                        newPayroll.WithholdingTax = reader.GetDouble(WithholdingTaxIndex);

                        newPayroll.NetPay = reader.GetDouble(NetpayIndex);
                        newPayroll.PayrollId = Payroll.GenerateId(newPayroll);

                        payrolls.Add(newPayroll);

                    } while (reader.Read());
                }
            }

            return payrolls;
        }


        private void FindCutoffDate(IExcelDataReader reader)
        {
            reader.Read();
            reader.Read();
            reader.Read();
            CheckCutoffDate(reader);
            reader.Read();
            CheckCutoffDate(reader);
            reader.Read();
        }
        private void CheckCutoffDate(IExcelDataReader reader)
        {
            if (CutoffDate == default)
            {
                string payrollDateRaw = "";
                if (reader.GetValue(0) is not null)
                    payrollDateRaw = reader.GetString(0).Split(':')[1].Trim();
                else if (reader.GetValue(1) is not null)
                    payrollDateRaw = reader.GetString(1).Trim().Replace("*", "").Trim();

                if (payrollDateRaw != "")
                    CutoffDate = DateTime.ParseExact(payrollDateRaw, "dd MMMM yyyy", CultureInfo.InvariantCulture);
            }
        }

        private static string[] ParseEmployeeDetail(IExcelDataReader reader, int nameIdx)
        {
            if (reader.GetValue(nameIdx) is not null)
            {
                var fullname_raw = reader.GetString(nameIdx).Trim(')').Split('(');
                if (fullname_raw.Length < 2)
                    return null;

                return new[] { fullname_raw[0].Trim(), fullname_raw[1].Trim() };
            }
            return null;
        }
    }
}
