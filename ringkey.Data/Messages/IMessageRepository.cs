using System.Collections.Generic;
using System.Threading.Tasks;
using ringkey.Common.Models;
using ringkey.Common.Models.Messages;

namespace ringkey.Data.Messages
{
    public interface IMessageRepository : IRepository<Message>
    {
        List<Message> GetLatest(int amount);
        List<Message> GetReplies(string id);
        Message GetById(string id);
        List<Message> GetUnprocessed();
        void ProcessedMessage(Message message);
    }
}