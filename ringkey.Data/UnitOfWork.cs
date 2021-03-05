using RethinkDb.Driver;
using RethinkDb.Driver.Model;
using ringkey.Data.Accounts;
using ringkey.Data.Messages;

namespace ringkey.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        public IAccountRepository Account { get; private set; }
        public IMessageRepository Message { get; private set; }

        public UnitOfWork()
        {
            var connection = RethinkDB.R
                .Connection()
                .Hostname("127.0.0.1")
                .Port(28015)
                .Timeout(60)
                .Connect();

            if (!RethinkDB.R.DbList().Contains("ringkey").Run(connection))
            {
                RethinkDB.R.DbCreate("ringkey").Run(connection);
                RethinkDB.R.Db("ringkey").TableCreate("Account").Run(connection);
                RethinkDB.R.Db("ringkey").TableCreate("Role").Run(connection);
                RethinkDB.R.Db("ringkey").TableCreate("Permission").Run(connection);
                RethinkDB.R.Db("ringkey").TableCreate("Message").Run(connection);
            }
            
            Account = new AccountRepository(connection);
            Message = new MessageRepository(connection);
        }
    }
}