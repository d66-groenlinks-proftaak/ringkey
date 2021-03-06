﻿using ringkey.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ringkey.Data.Reports
{
    public interface IReportRepository : IRepository<Report>
    {
        List<Report> GetByPostId(Guid postId);
    }
}
