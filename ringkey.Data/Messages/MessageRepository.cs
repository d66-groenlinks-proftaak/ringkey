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
                .OrderByDescending(msg => msg.Created)
                .Take(10)
                .Include(msg => msg.Author)
                .ToList();
        }

        public List<Message> GetReplies(string id)
        {
            return _dbContext.Message
                .Where(msg => msg.Type == MessageType.Reply && msg.Parent == id && msg.Processed)
                .OrderByDescending(msg => msg.Created)
                .Include(msg => msg.Author)
                .ToList();
        }

        public Message GetById(string id)
        {
            return _dbContext.Message.Include(msg => msg.Author).FirstOrDefault(msg => msg.Id.ToString() == id && msg.Processed);
        }

        public List<Message> GetUnprocessed()
        {
            return _dbContext.Message
                .Where(msg => !msg.Processed)
                .OrderByDescending(msg => msg.Created)
                .Include(msg => msg.Author)
                .ToList();
        }

        public MessageRepository(RingkeyDbContext context) : base(context)
        {
        }

    }
}