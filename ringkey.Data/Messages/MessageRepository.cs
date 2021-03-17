using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ringkey.Common.Models;
using ringkey.Common.Models.Messages;

namespace ringkey.Data.Messages
{
    public class MessageRepository : Repository<Message>, IMessageRepository
    {
        public List<Message> GetLatest(int amount)
        {
            return _dbContext.Message
                .Where(msg => msg.Type == MessageType.Thread && msg.Processed)
                .OrderByDescending(msg => msg.Pinned)
                .ThenByDescending(msg => msg.Created)
                .Include(msg => msg.Parent)
                .Take(10)
                .Include(msg => msg.Author)
                .ToList();
        }
        
        public List<Message> GetOldest(int amount)
        {
            return _dbContext.Message
                .Where(msg => msg.Type == MessageType.Thread && msg.Processed)
                .OrderByDescending(msg => msg.Pinned)
                .ThenBy(msg => msg.Created)
                .Include(msg => msg.Parent)
                .Take(10)
                .Include(msg => msg.Author)
                .ToList();
        }
        
        public List<Message> GetTop(int amount)
        {
            long lastSeven = DateTimeOffset.Now.AddDays(-7).ToUnixTimeMilliseconds();
            
            return _dbContext.Message
                .Where(msg => msg.Type == MessageType.Thread && msg.Processed && msg.Created > lastSeven)
                .OrderByDescending(msg => msg.Pinned)
                .ThenByDescending(msg => msg.Views)
                .Include(msg => msg.Parent)
                .Take(10)
                .Include(msg => msg.Author)
                .ToList();
        }

        public bool IsGuest(string id)
        {
            // ReSharper disable once PossibleNullReferenceException
            return _dbContext.Message
                .Include(msg => msg.Author)
                .ThenInclude(user => user.Roles)
                .FirstOrDefault(msg => msg.Id.ToString() == id)
                .Author.Roles.Any(role => role.Type == RoleType.Guest);
        }

        public int GetReplyCount(string id)
        {
            return _dbContext.Message
                .Count(msg => msg.Parent == _dbContext.Message.FirstOrDefault(_msg => _msg.Id.ToString() == id));
        }

        public List<MessageTag> GetCategories()
        {
            return _dbContext.Tag.Where(tag => tag.Type == MessageTagType.Category).ToList();
        }

        public List<Message> GetReplies(string id)
        {
            return _dbContext.Message
                .Where(msg => msg.Type == MessageType.Reply && msg.Parent == _dbContext.Message.FirstOrDefault(_msg => _msg.Id.ToString() == id) && msg.Processed)
                .OrderByDescending(msg => msg.Created)
                .Include(msg => msg.Author)
                .Include(msg => msg.Parent)
                .ToList();
        }

        public Message GetById(string id)
        {
            Message msg = _dbContext.Message
                .Include(msg => msg.Author)
                .Include(msg => msg.Children)
                .FirstOrDefault(msg => msg.Id.ToString() == id && msg.Processed);

            if (msg != null)
                msg.Views++;

            _dbContext.SaveChanges();

            return msg;
        }

        public List<Message> GetUnprocessed()
        {
            return _dbContext.Message
                .Where(msg => !msg.Processed)
                .OrderByDescending(msg => msg.Created)
                .Include(msg => msg.Author)
                .Include(msg => msg.Parent)
                .ToList();
        }

        public MessageRepository(RingkeyDbContext context) : base(context)
        {
        }

    }
}