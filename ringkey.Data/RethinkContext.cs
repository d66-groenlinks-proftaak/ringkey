using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RethinkDb.Driver;
using RethinkDb.Driver.Net;

namespace ringkey.Data
{
    public class RethinkContext : IRethinkContext
    {
        private List<Func<Task>> _commands = new List<Func<Task>>();
        public Connection Connection { get; set; }

        public RethinkContext()
        {
            Connection = RethinkDB.R
                .Connection()
                .Hostname("127.0.0.1")
                .Port(28015)
                .Timeout(60)
                .Connect();
        }

        public void AddCommand(Func<Task> func)
        {
            _commands.Add(func);
        }

        public void SaveChanges()
        {
            foreach (Func<Task> command in _commands)
                command();
            
            _commands.Clear();
        }
    }
}