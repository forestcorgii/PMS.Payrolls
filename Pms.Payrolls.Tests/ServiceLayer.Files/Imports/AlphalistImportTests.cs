using Xunit;
using Pms.Payrolls.ServiceLayer.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pms.Payrolls.Domain;

namespace Pms.Payrolls.ServiceLayer.Files.Tests
{
    public class AlphalistImportTests
    {
        [Fact()]
        public void ShouldImportAlphalistsToBirProgram()
        {
            CompanyView company = new();
            string alphalistFilepath = $@"{AppDomain.CurrentDomain.BaseDirectory}\TESTDATA\AlphalistImportTests\INTERNATIONAL DATA CONVERSION SOLUTIONS INC_2022-Alpha(14).xls";
            string birDbfFilepath = @"C:\BIRALPHA70\DATA";

            AlphalistImport importer = new();
            importer.ImportToBIRProgram(alphalistFilepath, birDbfFilepath, company, 2022);
        }
    }
}