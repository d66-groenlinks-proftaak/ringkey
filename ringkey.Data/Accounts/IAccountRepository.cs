using System.Threading.Tasks;
using RethinkDb.Driver.Model;
using RethinkDb.Driver.Net;
using ringkey.Common.Models;

namespace ringkey.Data.Accounts
{
    public interface IAccountRepository : IRepository<Account>
    {
        Task<Cursor<Change<Account>>> AccountChange();
        void GiveCredits(Account account, int amount);
        void Update(Account account);
    }
}