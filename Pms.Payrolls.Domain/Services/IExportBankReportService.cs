﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pms.Payrolls.Domain.Services
{
    public interface IExportBankReportService
    {
        void StartExport(IEnumerable<Payroll> payrolls);

    }
}
