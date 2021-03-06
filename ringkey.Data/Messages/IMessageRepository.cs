﻿using System.Collections.Generic;
using System.Threading.Tasks;
using ringkey.Common.Models;
using ringkey.Common.Models.Messages;

namespace ringkey.Data.Messages
{
    public interface IMessageRepository : IRepository<Message>
    {
        List<Message> GetLatest(int amount);
        List<Message> GetLatestWithTag(string tag, int amount);
        List<Message> GetAnnouncement();
        List<Message> GetOldest(int amount);
        List<Message> GetOldestWithTag(string tag, int amount);
        List<Message> GetTop(int amount);
        List<Message> GetTopWithTag(string tag, int amount);
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
        void AddAnnouncement(string PostId);
        void RemoveAnnouncement(string PostId);
        void CreateNewRating(string PostId, MessageRatingType type, Account account);
        void removeRating(string PostId, Account account);
        MessageRating GetMessageRatingById(string PostId, Account account);
        List<MessageRating> GetMessageRating(string PostId, MessageRatingType ratingType);
    }
}