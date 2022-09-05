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
    public class PayrollRegisterPDImport : IImportPayrollService
    {
        private int EEIdIndex = -1;
        private int RegularHoursIndex = -1;
        private int AbsentAndTardyIndex = -1;
        private int GrossIndex = -1;
        private int RegularPayIndex = -1;
        private int NetPayIndex = -1;

        private int OvertimeIndex = -1;
        private int RestDayOvertimeIndex = -1;
        private int HolidayOvertimeIndex = -1;
        private int NightDifferentialIndex = -1;

        private int EmployeeSSSIndex = -1;
        private int EmployeePagibigIndex = -1;
        private int EmployeePhilHealthIndex = -1;

        private int WithholdingTaxIndex = -1;

        private DateTime CutoffDate { get; set; }

        private string PayrollRegisterFilePath { get; set; }

        public void ValidatePayRegisterFile()
        {
            if (EEIdIndex == -1) throw new PayrollRegisterHeaderNotFoundException( "Employee ID",PayrollRegisterFilePath);
            if (RegularHoursIndex == -1) throw new PayrollRegisterHeaderNotFoundException( "Regular Hours",PayrollRegisterFilePath);
            if (OvertimeIndex == -1) throw new PayrollRegisterHeaderNotFoundException( "Overtime",PayrollRegisterFilePath);
            if (RestDayOvertimeIndex == -1) throw new PayrollRegisterHeaderNotFoundException( "Restdat Overtime",PayrollRegisterFilePath);
            if (HolidayOvertimeIndex == -1) throw new PayrollRegisterHeaderNotFoundException( "Holiday Overtime",PayrollRegisterFilePath);
            if (NightDifferentialIndex == -1) throw new PayrollRegisterHeaderNotFoundException( "Night Differential",PayrollRegisterFilePath);
            if (AbsentAndTardyIndex == -1) throw new PayrollRegisterHeaderNotFoundException( "AbsTar",PayrollRegisterFilePath);
            
            if (RegularPayIndex== -1) throw new PayrollRegisterHeaderNotFoundException( "Regular Pay", PayrollRegisterFilePath);
            if (GrossIndex == -1) throw new PayrollRegisterHeaderNotFoundException( "Gross Pay", PayrollRegisterFilePath);
            if (NetPayIndex == -1) throw new PayrollRegisterHeaderNotFoundException("Net Pay", PayrollRegisterFilePath);
            
            if (EmployeeSSSIndex == -1) throw new PayrollRegisterHeaderNotFoundException("SSS EE", PayrollRegisterFilePath);
            if (EmployeePagibigIndex == -1) throw new PayrollRegisterHeaderNotFoundException("Pagibig EE", PayrollRegisterFilePath);
            if (EmployeePhilHealthIndex == -1) throw new PayrollRegisterHeaderNotFoundException("PhilHealth EE", PayrollRegisterFilePath);

            if (WithholdingTaxIndex == -1) throw new PayrollRegisterHeaderNotFoundException( "Withholding Tax",PayrollRegisterFilePath);

            if (CutoffDate == default) throw new PayrollRegisterHeaderNotFoundException("Cutoff Date", PayrollRegisterFilePath);
        }


        public IEnumerable<Payroll> StartImport(string payrollRegisterFilePath)
        {
            PayrollRegisterFilePath = payrollRegisterFilePath;

            List<Payroll> payrolls = new();
            using (var stream = File.Open(PayrollRegisterFilePath, FileMode.Open, FileAccess.Read))
            {
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    FindHeaders(reader);
                    FindPayrollDate(reader);

                    ValidatePayRegisterFile();

                    Cutoff cutoff = new Cutoff(CutoffDate);
                    do
                    {
                        string employee_id = "";
                        if (EEIdIndex > -1)
                        {
                            if (reader.GetString(EEIdIndex) is null)
                                continue;
                            employee_id = reader.GetString(EEIdIndex).Trim();
                        }

                        var newPayroll = new Payroll()
                        {
                            EEId = employee_id,
                            CutoffId = cutoff.CutoffId,
                            YearCovered = cutoff.YearCovered,
                        };

                        newPayroll.RegularPay = reader.GetDouble(RegularPayIndex);
                        newPayroll.GrossPay = reader.GetDouble(GrossIndex);
                        newPayroll.NetPay = reader.GetDouble(NetPayIndex);

                        newPayroll.RegHours = reader.GetDouble(RegularHoursIndex);
                        newPayroll.Overtime = reader.GetDouble(OvertimeIndex);
                        newPayroll.RestDayOvertime = reader.GetDouble(RestDayOvertimeIndex);
                        newPayroll.HolidayOvertime = reader.GetDouble(HolidayOvertimeIndex);
                        newPayroll.NightDifferential = reader.GetDouble(NightDifferentialIndex);
                        newPayroll.AbsTar = reader.GetDouble(AbsentAndTardyIndex);

                        if (cutoff.CutoffDate.Day != 15)
                        {
                            newPayroll.EmployeeSSS = reader.GetDouble(EmployeeSSSIndex);
                            newPayroll.EmployeePagibig = reader.GetDouble(EmployeePagibigIndex);
                            newPayroll.EmployeePhilHealth = reader.GetDouble(EmployeePhilHealthIndex);
                            newPayroll.WithholdingTax = reader.GetDouble(WithholdingTaxIndex);
                        }

                        newPayroll.PayrollId = Payroll.GenerateId(newPayroll);
                        newPayroll.UpdateValues();
                        payrolls.Add(newPayroll);
                    } while (reader.Read());
                }
            }

            return payrolls;
        }
        private void FindHeaders(IExcelDataReader reader)
        {
            reader.Read();
            CheckHeaders(reader);
            reader.Read();
            CheckHeaders(reader);
            reader.Read();
            CheckHeaders(reader);
            reader.Read();
        }
        private void CheckHeaders(IExcelDataReader reader)
        {
            FindHeaderColumnIndex(ref EEIdIndex, "ID", reader);
            FindHeaderColumnIndex(ref GrossIndex, "GROSS", reader);
            FindHeaderColumnIndex(ref RegularPayIndex, "REG_PAY", reader);
            FindHeaderColumnIndex(ref NetPayIndex, "NET", reader);

            FindHeaderColumnIndex(ref RegularHoursIndex, "REG", reader);
            FindHeaderColumnIndex(ref OvertimeIndex, "R_OT", reader);
            FindHeaderColumnIndex(ref RestDayOvertimeIndex, "RD_OT", reader);
            FindHeaderColumnIndex(ref HolidayOvertimeIndex, "HOL_OT", reader);
            FindHeaderColumnIndex(ref NightDifferentialIndex, "ND", reader);
            FindHeaderColumnIndex(ref AbsentAndTardyIndex, "ABS_TAR", reader);

            FindHeaderColumnIndex(ref EmployeeSSSIndex, "SSS_EE", reader);
            FindHeaderColumnIndex(ref EmployeePagibigIndex, "ADJUST1", reader);
            FindHeaderColumnIndex(ref EmployeePhilHealthIndex, "PHIC_EE", reader);
            FindHeaderColumnIndex(ref EmployeePhilHealthIndex, "PHIC", reader);

            FindHeaderColumnIndex(ref WithholdingTaxIndex, "TAX", reader);

        }

        private void FindHeaderColumnIndex(ref int index, string header, IExcelDataReader reader)
        {
            if (index == -1)
            {
                for (int column = 0; column < reader.FieldCount; column++)
                {
                    if (reader.GetValue(column) is not null)
                        if ((reader.GetString(column).Trim().ToUpper() ?? "") == (header ?? ""))
                        {
                            index = column;
                            return;
                        }
                }
                index = -1;
            }
        }


        private void FindPayrollDate(IExcelDataReader reader)
        {
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
