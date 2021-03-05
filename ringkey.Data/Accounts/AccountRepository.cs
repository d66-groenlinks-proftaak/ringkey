using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ringkey.Common.Models;
using RethinkDb.Driver;
using RethinkDb.Driver.Net;
using RethinkDb.Driver.Linq;
using RethinkDb.Driver.Model;

namespace ringkey.Data.Accounts
{
    public class AccountRepository : Repository<Account>, IAccountRepository
    {
        public async Task<Cursor<Change<Account>>> AccountChange()
        {
            return await RethinkDB.R.Db("ringkey").Table(nameof(Account).Split(".")[^1]).Changes().RunChangesAsync<Account>(_connection);
        }

        public AccountRepository(IRethinkContext rethinkContext) : base(rethinkContext)
        {
        }
    }
}