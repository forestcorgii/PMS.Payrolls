using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pms.Payrolls.Domain
{
    public class CompanyView
    {
        public CompanyView() { }

        public CompanyView(string registeredName, string tIN, int branchCode, string region)
        {
            RegisteredName = registeredName;
            TIN = tIN;
            BranchCode = branchCode;
            Region = region;
        }

        public string CompanyId { get; set; }
        public string Site { get; set; }
        public string Acronym { get; set; }
        public string RegisteredName { get; set; }

        public string Region { get; set; }
        public string TIN { get; set; }
        public int BranchCode { get; set; }

        public double MinimumRate { get; set; }

        public override string ToString() => CompanyId;

    }
}
