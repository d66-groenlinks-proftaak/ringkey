using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ringkey.Common.Models;

namespace ringkey.Data.Accounts
{
    public class AccountRepository : Repository<Account>, IAccountRepository
    {
        public AccountRepository(RingkeyDbContext context) : base(context)
        {
        }
    }
}