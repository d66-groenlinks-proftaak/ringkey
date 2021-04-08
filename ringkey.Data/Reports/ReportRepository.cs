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
        public List<Report> GetByPostId(Guid postId)
        {
            return _dbContext.Report.Where(rep => rep.Message.Id == postId).ToList();
        }
        public ReportRepository(RingkeyDbContext context) : base(context)
        {
        }
    }
}
