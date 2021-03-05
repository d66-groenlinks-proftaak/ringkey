using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RethinkDb.Driver.Net;

namespace ringkey.Data
{
    public interface IRethinkContext
    {
        Connection Connection { get; set; }
        void AddCommand(Func<Task> func);
        void SaveChanges();
    }
}