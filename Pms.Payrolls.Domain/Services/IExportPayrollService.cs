﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pms.Payrolls.Domain.Services
{
    public interface IExportPayrollService
    {
        void StartExport(IEnumerable<Payroll> payrolls, string cutoffId, string companyName);

    }
}
