using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RethinkDb.Driver;
using RethinkDb.Driver.Model;
using RethinkDb.Driver.Net;
using ringkey.Data.Accounts;
using ringkey.Data.Messages;

namespace ringkey.Data
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        public IAccountRepository Account { get; private set; }
        public IMessageRepository Message { get; private set; }

        private Connection _connection;
        private IRethinkContext _rethinkContext;

        public UnitOfWork()
        {
            _rethinkContext = new RethinkContext();
            
            _connection = RethinkDB.R
                .Connection()
                .Hostname("127.0.0.1")
                .Port(28015)
                .Timeout(60)
                .Connect();

            if (!RethinkDB.R.DbList().Contains("ringkey").Run(_connection))
            {
                RethinkDB.R.DbCreate("ringkey").Run(_connection);
                RethinkDB.R.Db("ringkey").TableCreate("Account").Run(_connection);
                RethinkDB.R.Db("ringkey").TableCreate("Role").Run(_connection);
                RethinkDB.R.Db("ringkey").TableCreate("Permission").Run(_connection);
                RethinkDB.R.Db("ringkey").TableCreate("Message").Run(_connection);
            }
            
            Account = new AccountRepository(_rethinkContext);
            Message = new MessageRepository(_rethinkContext);
            
        }

        public void SaveChanges()
        {
            _rethinkContext.SaveChanges();
        }

        public void Dispose()
        {
            _connection.Dispose();
        }
    }
}