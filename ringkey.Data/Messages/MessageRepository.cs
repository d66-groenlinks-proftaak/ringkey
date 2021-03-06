﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ringkey.Common.Models;
using ringkey.Common.Models.Accounts;
using ringkey.Common.Models.Messages;

namespace ringkey.Data.Messages
{
    public class MessageRepository : Repository<Message>, IMessageRepository
    {
        public List<Message> GetLatest(int amount)
        {
            return _dbContext.Message
                .Where(msg => msg.Type == MessageType.Thread && msg.Processed && msg.Tags.Where(tag=> tag.Type == MessageTagType.Announcement).FirstOrDefault() == null)
                .OrderByDescending(msg => msg.Pinned)
                .ThenByDescending(msg => msg.Created)
                .Take(amount)
                .Include(msg => msg.Author)
                .ThenInclude(author => author.Roles)
                .Include(msg => msg.Parent)
                .Include(msg => msg.Children)
                .Include(msg => msg.Ratings)
                .ToList();

        }
        public List<Message> GetLatestWithTag(string tag, int amount)
        {
            return _dbContext.Message
                .Include(msg => msg.Tags)
                .Where(msg => msg.Tags
                .Any(a => a.Name == tag))
                .Where(msg => msg.Type == MessageType.Thread && msg.Processed)
                .OrderByDescending(msg => msg.Pinned)
                .ThenByDescending(msg => msg.Created)
                .Take(amount)
                .Include(msg => msg.Author)
                .ThenInclude(author => author.Roles)
                .Include(msg => msg.Parent)
                .Include(msg => msg.Children)
                .ToList();
        }

        public List<Message> GetAnnouncement()
        {
            return _dbContext.Message
                .Where(msg => msg.Tags.Where(o => o.Type == MessageTagType.Announcement).FirstOrDefault() != null)
                .Include(msg => msg.Parent)
                .Include(msg => msg.Author)
                .ToList();
        }

        public List<Message> GetOldest(int amount)
        {
            return _dbContext.Message
                .Where(msg => msg.Type == MessageType.Thread && msg.Processed && msg.Processed && msg.Tags.Where(o => o.Type == MessageTagType.Announcement).FirstOrDefault() == null)
                .OrderByDescending(msg => msg.Pinned)
                .ThenBy(msg => msg.Created)
                .Take(amount)
                .Include(msg => msg.Author)
                .ThenInclude(author => author.Roles)
                .Include(msg => msg.Parent)
                .Include(msg => msg.Children)
                .ToList();
        }

        public List<Message> GetOldestWithTag(string tag, int amount)
        {
            return _dbContext.Message
                .Where(msg => msg.Type == MessageType.Thread && msg.Processed && msg.Processed && msg.Tags.Where(o => o.Type == MessageTagType.Announcement).FirstOrDefault() == null && msg.Tags.Any(a => a.Name == tag))
                .OrderByDescending(msg => msg.Pinned)
                .ThenBy(msg => msg.Created)
                .Take(amount)
                .Include(msg => msg.Author)
                .ThenInclude(author => author.Roles)
                .Include(msg => msg.Parent)
                .Include(msg => msg.Children)
                .ToList();
        }
        
        public List<Message> GetTop(int amount)
        {
            long lastSeven = DateTimeOffset.Now.AddDays(-7).ToUnixTimeMilliseconds();
            
            return _dbContext.Message
                .Where(msg => msg.Type == MessageType.Thread && msg.Processed && msg.Created > lastSeven && msg.Processed && msg.Tags.Where(o => o.Type == MessageTagType.Announcement).FirstOrDefault() == null)
                .OrderByDescending(msg => msg.Pinned)
                .ThenByDescending(msg => msg.Views)
                .Take(amount)
                .Include(msg => msg.Author)
                .ThenInclude(author => author.Roles)
                .Include(msg => msg.Parent)
                .Include(msg => msg.Children)
                .ToList();
        }

        public List<Message> GetTopWithTag(string tag, int amount)
        {
            long lastSeven = DateTimeOffset.Now.AddDays(-7).ToUnixTimeMilliseconds();

            return _dbContext.Message
                .Where(msg => msg.Type == MessageType.Thread && msg.Processed && msg.Created > lastSeven && msg.Processed && msg.Tags.Where(o => o.Type == MessageTagType.Announcement).FirstOrDefault() == null && msg.Tags.Any(a => a.Name == tag))
                .OrderByDescending(msg => msg.Pinned)
                .ThenByDescending(msg => msg.Views)
                .Take(amount)
                .Include(msg => msg.Author)
                .ThenInclude(author => author.Roles)
                .Include(msg => msg.Parent)
                .Include(msg => msg.Children)
                .ToList();
        }



        public bool IsGuest(string id)
        {
            // ReSharper disable once PossibleNullReferenceException
            return _dbContext.Message
                .Include(msg => msg.Author)
                .ThenInclude(user => user.Roles)
                .FirstOrDefault(msg => msg.Id.ToString() == id)
                .Author.Roles.Any(role => role.Name == "Guest");
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
            List<Message> messages = _dbContext.Message
                .Where(msg => msg.Type == MessageType.Reply && msg.Parent == _dbContext.Message.FirstOrDefault(_msg => _msg.Id.ToString() == id) && msg.Processed)
                .OrderByDescending(msg => msg.Created)
                .Include(msg => msg.Author)
                .Include(msg => msg.Parent)
                .ToList();

            return messages;
        }


        public List<ThreadView> GetReplyChildren(string id)
        {
            return _dbContext.Message
                .Where(msg => msg.Type == MessageType.Reply && msg.Parent == _dbContext.Message.FirstOrDefault(_msg => _msg.Id.ToString() == id) && msg.Processed)
                .OrderByDescending(msg => msg.Created)
                .Take(3)
                .Include(msg => msg.Author).Select(c => c.GetThreadView()).ToList();
        }

        public List<Message> GetNextReplies(string id)
        {
            Message msg = GetById(id);

            if (msg == null || msg.Parent == null)
                return new List<Message>();
            
            List<Message> MessageList = _dbContext.Message
                .Where(_msg => _msg.Created < msg.Created && _msg.Parent == msg.Parent)
                .OrderByDescending(msg => msg.Created)
                .Take(4)
                .ToList();

            return MessageList;
        }

        public Message GetById(string id, bool requiresProcessed = true)
        {
            Message msg = _dbContext.Message
                .Include(msg => msg.Author)
                .ThenInclude(author => author.Roles)
                .Include(msg => msg.Children)
                .Include(msg => msg.Parent)
                .Include(msg => msg.Attachments)
                .Include(msg => msg.Tags)
                .FirstOrDefault(msg => msg.Id.ToString() == id && (!requiresProcessed || msg.Processed));

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
                .ThenInclude(author => author.Roles)
                .Include(msg => msg.Children)
                .Include(msg => msg.Parent)
                .ToList();
        }

        public List<Message> GetShadowBannedMessages()
        {
            return _dbContext.Message
                .Where(msg => msg.Type == MessageType.PossibleSpam)
                .OrderBy(msg => msg.Created)
                .Take(5)
                .Include(msg => msg.Parent)
                .Include(msg => msg.Author)
                .ThenInclude(a => a.Roles)
                .ToList();
        }

        public Message GetMessageById(string PostId)
        {
            return _dbContext.Message
                .Where(a => a.Id.ToString() == PostId).FirstOrDefault();
        }

        public MessageRating GetMessageRatingById(string PostId, Account account)
        {
            if (_dbContext.MessageRating.Where(rating => rating.Message.Id.ToString() == PostId && rating.Account == account).FirstOrDefault() != null)
            {
                return _dbContext.MessageRating.Where(rating => rating.Message.Id.ToString() == PostId && rating.Account == account).FirstOrDefault();
            }
            return new MessageRating();
        }

        public List<MessageRating> GetMessageRating(string PostId, MessageRatingType ratingType)
        {
            return _dbContext.MessageRating.Where(rating => rating.Message.Id.ToString() == PostId && rating.Type == ratingType).ToList();
        }

        public void LockMessage(string PostId)
        {
            if (_dbContext.Tag.Where(tag => tag.Message.Id.ToString() == PostId).FirstOrDefault() == null) {
                _dbContext.Tag.Add(new MessageTag() { Message = _dbContext.Message.Where(msg => msg.Id.ToString() == PostId).FirstOrDefault(), Id = Guid.NewGuid(), Type = MessageTagType.Lock, Name = "Lock" });
                _dbContext.SaveChanges();

            }
            else if (_dbContext.Tag.Where(tag => tag.Message.Id.ToString() == PostId).FirstOrDefault().Type != MessageTagType.Lock)
            {
                _dbContext.Tag.Add(new MessageTag() { Message = _dbContext.Message.Where(msg => msg.Id.ToString() == PostId).FirstOrDefault(), Id = Guid.NewGuid(), Type = MessageTagType.Lock, Name = "Lock" });
                _dbContext.SaveChanges();
            }
            else if (_dbContext.Tag.Where(tag => tag.Message.Id.ToString() == PostId).FirstOrDefault().Type == MessageTagType.Lock)
            {
                _dbContext.Tag.Remove(_dbContext.Tag.Where(tag => tag.Message.Id.ToString() == PostId && tag.Type == MessageTagType.Lock).FirstOrDefault());
                _dbContext.SaveChanges();

            }
            

        }

        public void PinMessage(string PostId)
        {
            if (_dbContext.Tag.Where(tag => tag.Message.Id.ToString() == PostId).FirstOrDefault() == null)
            {
                _dbContext.Tag.Add(new MessageTag() { Message = _dbContext.Message.Where(msg => msg.Id.ToString() == PostId).FirstOrDefault(), Id = Guid.NewGuid(), Type = MessageTagType.Pin, Name = "Pin" });
                _dbContext.SaveChanges();

            }
            else if (_dbContext.Tag.Where(tag => tag.Message.Id.ToString() == PostId).FirstOrDefault().Type != MessageTagType.Pin)
            {
                _dbContext.Tag.Add(new MessageTag() { Message = _dbContext.Message.Where(msg => msg.Id.ToString() == PostId).FirstOrDefault(), Id = Guid.NewGuid(), Type = MessageTagType.Pin, Name = "Pin" });
                _dbContext.SaveChanges();
            }
            else if (_dbContext.Tag.Where(tag => tag.Message.Id.ToString() == PostId).FirstOrDefault().Type == MessageTagType.Pin)
            {
                _dbContext.Tag.Remove(_dbContext.Tag.Where(tag => tag.Message.Id.ToString() == PostId && tag.Type == MessageTagType.Pin).FirstOrDefault());
                _dbContext.SaveChanges();

            }

        }

        public void AddAnnouncement(string PostId)
        {
            _dbContext.Tag.Add(new MessageTag() { Message = _dbContext.Message.Where(msg => msg.Id.ToString() == PostId).FirstOrDefault(), Id = Guid.NewGuid(), Type = MessageTagType.Announcement, Name = "Announcement" });
            _dbContext.SaveChanges();
        }

        public void RemoveAnnouncement(string PostId)
        {
            _dbContext.Tag.Remove(_dbContext.Tag.Where(tag => tag.Message.Id.ToString() == PostId && tag.Type == MessageTagType.Announcement).FirstOrDefault());
            _dbContext.SaveChanges();

        }

        public void CreateNewRating(string PostId, MessageRatingType type, Account account)
        {
            _dbContext.MessageRating.Add(new MessageRating() { Account = account, Id = Guid.NewGuid(), Message = _dbContext.Message.Where(m => m.Id.ToString() == PostId).FirstOrDefault(), Type = type });
            _dbContext.SaveChanges();
        }

        public void removeRating(string PostId, Account account)
        {
            _dbContext.MessageRating.Remove(_dbContext.MessageRating.Where(rating => rating.Message.Id.ToString() == PostId && rating.Account == account).FirstOrDefault());
            _dbContext.SaveChanges();

        }

        public MessageRepository(RingkeyDbContext context) : base(context)
        {
        }

    }
}