using System.Threading.Tasks;
using ringkey.Common.Models;

namespace ringkey.Data.Accounts
{
    public interface IAccountRepository : IRepository<Account>
    {
        Account GetByEmail(string email);
        Account GetById(string id);
    }
}