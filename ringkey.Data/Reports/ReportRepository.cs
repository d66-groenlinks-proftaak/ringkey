using ringkey.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ringkey.Data.Reports
{
    public class ReportRepository : Repository<Report>, IReportRepository
    {
        
        public ReportRepository(RingkeyDbContext context) : base(context)
        {
        }
    }
}
