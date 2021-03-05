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
        public void GiveCredits(Account account, int amount)
        {
            RethinkDB.R.Db("ringkey").Table(nameof(Account).Split(".")[^1]).Filter(new
            {
                id = account.id
            }).Update(new
            {
                Credits = account.Credits + amount
            }).Run(_connection);
        }

        public void Update(Account account)
        {
            RethinkDB.R.Db("ringkey").Table(nameof(Account).Split(".")[^1]).Filter(new
            {
                id = account.id
            }).Update(account).Run(_connection);
        }

        public async Task<Cursor<Change<Account>>> AccountChange()
        {
            return await RethinkDB.R.Db("ringkey").Table(nameof(Account).Split(".")[^1]).Changes().RunChangesAsync<Account>(_connection);
        }

        public AccountRepository(Connection connection) : base(connection)
        {
        }
    }
}