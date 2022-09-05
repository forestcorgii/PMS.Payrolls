using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pms.Payrolls.Domain
{
    public class Company
    {
        public string CompanyId { get; set; }
        public string Site { get; set; }
        public string Acronym { get; set; }
        public string RegisteredName { get; set; }
        public string Region { get; set; }
        public string TIN { get; set; }
        public string BranchCode { get; set; }

        public double MinimumRate { get; set; }

        public static string GenerateId(Company company) => $"{company.Site}{company.Acronym}{company.BranchCode}";

        public override string ToString()
        {
            return CompanyId;
        }
    }
}
