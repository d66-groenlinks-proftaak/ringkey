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
            Account = new AccountRepository();
            Message = new MessageRepository();
        }
    }
}