using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pms.Payrolls.Domain
{
    public class CompanyView
    {
        public string CompanyId { get; private set; }
        public string Site { get; private set; }
        public string Acronym { get; private set; }
        public string RegisteredName { get; private set; }
        public string Region { get; private set; }
        public string TIN { get; private set; }
        public int BranchCode { get; private set; }

        public double MinimumRate { get; private set; }

        public override string ToString() => CompanyId;

    }
}
