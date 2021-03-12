using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ringkey.Common.Models;

namespace ringkey.Data.Accounts
{
    public class AccountRepository : Repository<Account>, IAccountRepository
    {        
        public Account GetByEmail(string email)
        {
            return _dbContext.Account.Include(acc => acc.Roles).FirstOrDefault(acc => acc.Email == email);
        }

        public Account GetById(string id)
        {
            return _dbContext.Account.Include(acc => acc.Roles).FirstOrDefault(acc => acc.Id.ToString() == id);
        }
        
        public AccountRepository(RingkeyDbContext context) : base(context)
        {
        }
    }
}