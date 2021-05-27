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
            return _dbContext.Account.Include(acc => acc.Roles).ThenInclude(role => role.Permissions).FirstOrDefault(acc => acc.Email == email);
        }

        public Account GetById(string id)
        {
            return _dbContext.Account.Include(acc => acc.Roles).ThenInclude(role => role.Permissions).FirstOrDefault(acc => acc.Id.ToString() == id);
        }

        public Profile GetProfileById(string id)
        {
            Account account = _dbContext.Account
                .Include(acc => acc.Roles)
                .FirstOrDefault(acc => acc.Id.ToString() == id);

            List<Message> _messages = new List<Message>();
            List<Message> messages = _dbContext.Message
                .Include(msg => msg.Author)
                .Where(msg => msg.Processed && msg.Type == MessageType.Thread && msg.Author.Id.ToString() == id)
                .OrderByDescending(msg => msg.Created)
                .Take(10)
                .ToList();
            
            foreach (var message in messages)
            {
                _messages.Add(new Message()
                {
                    Id = message.Id,
                    Created = message.Created,
                    Content = message.Content,
                    Title = message.Title
                });
            }
            

            return new Profile()
            {
                Id = account.Id,
                FirstName = account.FirstName,
                LastName = account.LastName,
                Email = account.Email,
                Biography = account.Biography,
                Avatar = account.ProfilePicture,
                Messages = _messages
            };
        }
        
        public AccountRepository(RingkeyDbContext context) : base(context)
        {
        }
    }
}