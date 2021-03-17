using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ringkey.Common.Models;
using ringkey.Common.Models.Messages;

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

        public Profile GetProfileById(string id)
        {
            Account account = _dbContext.Account
                .Include(acc => acc.Roles)
                .FirstOrDefault(acc => acc.Id.ToString() == id);

            List<Message> messages = _dbContext.Message
                .Include(msg => msg.Author)
                .Where(msg => msg.Processed && msg.Type == MessageType.Thread && msg.Author.Id.ToString() == id)
                .OrderByDescending(msg => msg.Created)
                .Take(10)
                .ToList();

            foreach (Message message in messages)
            {
                message.Author = null;
            }

            foreach (Role role in account.Roles)
            {
                role.Account = null;
            }

            return new Profile()
            {
                Id = account.Id,
                FirstName = account.FirstName,
                LastName = account.LastName,
                Email = account.Email,
                Roles = account.Roles,
                Messages = messages
            };
        }
        
        public AccountRepository(RingkeyDbContext context) : base(context)
        {
        }
    }
}