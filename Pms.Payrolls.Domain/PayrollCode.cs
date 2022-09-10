using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Pms.Payrolls.Domain.Enums;

namespace Pms.Payrolls.Domain
{
    public class PayrollCode
    {
        public string PayrollCodeId { get; set; }

        public string CompanyId { get; set; }
        public Company Company { get; set; }

        public SiteChoices Site { get; set; }
    }
}
