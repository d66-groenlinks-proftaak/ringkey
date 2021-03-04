using RethinkDb.Driver.Model;
using ringkey.Data.Accounts;

namespace ringkey.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        public IAccountRepository Account { get; private set; }

        public UnitOfWork()
        {
            Account = new AccountRepository();
        }
    }
}