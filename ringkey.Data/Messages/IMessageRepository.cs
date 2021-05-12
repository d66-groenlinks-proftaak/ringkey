using System.Collections.Generic;
using System.Threading.Tasks;
using ringkey.Common.Models;
using ringkey.Common.Models.Messages;

namespace ringkey.Data.Messages
{
    public interface IMessageRepository : IRepository<Message>
    {
        List<Message> GetLatest(int amount);
        List<Message> GetAnnouncement();
        List<Message> GetOldest(int amount);
        List<Message> GetTop(int amount);
        List<Message> GetReplies(string id);
        Message GetById(string id, bool requiresProcessing = true);
        List<Message> GetUnprocessed();
        int GetReplyCount(string id);
        bool IsGuest(string id);
        Message GetMessageById(string PostId);
        List<Message> GetNextReplies(string id);
        List<ThreadView> GetReplyChildren(string id);
        void LockMessage(string postId);
        List<Message> GetShadowBannedMessages();
        void PinMessage(string PostId);
    }
}