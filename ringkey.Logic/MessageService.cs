using System;
using System.Collections.Generic;
using System.Linq;
using ringkey.Common.Models;
using ringkey.Common.Models.Accounts;
using ringkey.Common.Models.Messages;
using ringkey.Data;
using ringkey.Logic.Messages;
using Utility = ringkey.Logic.Accounts.Utility;

namespace ringkey.Logic
{
    public class MessageService
    {
        private UnitOfWork _unitOfWork;
        
        public MessageService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            }

        public MessageErrors CreateMessage(NewMessage message, Account authenticated, List<Attachment> attachments)
        {
            MessageErrors error;
            Account account = _unitOfWork.Account.GetByEmail(message.Email);

            if (authenticated != null)
            {
                account = _unitOfWork.Account.GetById(authenticated.Id.ToString());
            }
            else
            {
                if (account != null)
                {
                    error = Messages.Utility.CheckMessage(message, false);

                    if (error != MessageErrors.NoError)
                        return error;

                    if (account.Roles.Any(role => role.Name != "Guest"))
                    {
                        if (authenticated != null && authenticated.Id == account.Id)
                        {

                        }
                        else
                            return MessageErrors.EmailAlreadyOwned;
                    }
                }
                else
                {
                    error = Messages.Utility.CheckMessage(message);
                    
                    if (error != MessageErrors.NoError)
                        return error;

                    account = new Account()
                    {
                        Email = message.Email,
                        Password = "",
                        FirstName = message.Author,
                        LastName = "",
                        Roles = new List<Role>() 
                        { 
                            _unitOfWork.Role.GetByName("Guest")
                        }
                    };

                    _unitOfWork.Account.Add(account);
                }
            }

            Message newMessage = new Message()
            {
                Author = account,
                Content = Messages.Utility.SanitizeContent(message.Content),
                Created = DateTimeOffset.Now.ToUnixTimeMilliseconds(),
                Type = MessageType.Thread,
                Title = message.Title,
                Processed = false,
                Pinned = false,
                Attachments = attachments
            };

            _unitOfWork.Message.Add(newMessage);

            _unitOfWork.SaveChanges();

            return MessageErrors.NoError;
        }

        public Message GetMessageDetails(string id)
        {
            return _unitOfWork.Message.GetById(id);
        }

        public List<ThreadView> GetNextReplies(string id)
        {
            Message parent = _unitOfWork.Message.GetById(id)?.Parent;
            List<ThreadView> SendNew = new List<ThreadView>();

            if (parent != null)
            {
                SendNew = _unitOfWork.Message.GetNextReplies(id).Select(msg => new ThreadView {
                    Author = $"{msg.Author.FirstName} {msg.Author.LastName}",
                    AuthorId = msg.Author.Id.ToString(),
                    Content = msg.Content,
                    Id = msg.Id,
                    Parent = msg.Parent?.Id.ToString(),
                    Title = msg.Title,
                    Created = msg.Created,
                    Pinned = msg.Pinned,
                    Guest = _unitOfWork.Message.IsGuest(msg.Id.ToString()),
                    Replies = 0,
                }).ToList();
            }

            return SendNew;
        }

        public MessageErrors CreateReply(NewReply message, Account authenticated)
        {
            MessageErrors error;
            Account account = _unitOfWork.Account.GetByEmail(message.Email);

            if (authenticated != null)
            {
                account = _unitOfWork.Account.GetById(authenticated.Id.ToString());
            }
            else
            {
                if (account != null)
                {
                    error = Messages.Utility.CheckMessage(message, false);

                    if (error != MessageErrors.NoError)
                        return error;

                    if (account.Roles.Any(role => role.Name != "Guest"))
                    {
                        if (authenticated != null && authenticated.Id == account.Id)
                        {

                        }
                        else
                            return MessageErrors.EmailAlreadyOwned;
                    }
                }
                else
                {
                    error = Messages.Utility.CheckMessage(message);
                    
                    if (error != MessageErrors.NoError)
                        return error;

                    account = new Account()
                    {
                        Email = message.Email,
                        Password = "",
                        FirstName = message.Author,
                        LastName = "",
                        Roles = new List<Role>()
                        {
                            _unitOfWork.Role.GetByName("Guest")
                        }
                    };

                    _unitOfWork.Account.Add(account);
                }
            }

            Message parent = _unitOfWork.Message.GetById(message.Parent);
            if (parent.Type != MessageType.Thread)
            {
                if(parent.Parent == null || parent.Parent.Type != MessageType.Thread)
                    return MessageErrors.CannotCreateSubReply;
            }

            Message newMessage = new Message()
            {
                Author = account,
                Content = Messages.Utility.SanitizeContent(message.Content),
                Created = DateTimeOffset.Now.ToUnixTimeMilliseconds(),
                Type = MessageType.Reply,
                Parent = parent,
                Title = null,
                Processed = false,
                Pinned = false
            };

            _unitOfWork.Message.Add(newMessage);

            _unitOfWork.SaveChanges();

            return MessageErrors.NoError;
        }

        public List<ThreadView> GetLatest(int amount, MessageSortType sort = MessageSortType.New)
        {
            if(sort == MessageSortType.Top)
                return GetTop(10);
            if(sort == MessageSortType.Old)
                return GetOldest(10);
            
            List<Message> messages = _unitOfWork.Message.GetLatest(amount);
            List<ThreadView> replies = new List<ThreadView>();
            
            foreach(Message msg in messages)
            {
                replies.Add(new ThreadView()
                {
                    Author = $"{msg.Author.FirstName} {msg.Author.LastName}",
                    AuthorId = msg.Author.Id.ToString(),
                    Content = msg.Content,
                    Id = msg.Id,
                    Parent = msg.Parent?.Id.ToString(),
                    Title = msg.Title,
                    Created = msg.Created,
                    Pinned = msg.Pinned,
                    Guest = _unitOfWork.Message.IsGuest(msg.Id.ToString()),
                    Replies = _unitOfWork.Message.GetReplyCount(msg.Id.ToString())
                });
            }
            
            return replies;
        }
        
        public List<ThreadView> GetOldest(int amount)
        {
            List<Message> messages = _unitOfWork.Message.GetOldest(amount);
            List<ThreadView> replies = new List<ThreadView>();
            
            foreach(Message msg in messages)
            {
                replies.Add(new ThreadView()
                {
                    Author = $"{msg.Author.FirstName} {msg.Author.LastName}",
                    AuthorId = msg.Author.Id.ToString(),
                    Content = msg.Content,
                    Id = msg.Id,
                    Parent = msg.Parent?.Id.ToString(),
                    Title = msg.Title,
                    Created = msg.Created,
                    Pinned = msg.Pinned,
                    Guest = _unitOfWork.Message.IsGuest(msg.Id.ToString()),
                    Replies = _unitOfWork.Message.GetReplyCount(msg.Id.ToString())
                });
            }
            
            return replies;
        }
        
        public List<ThreadView> GetTop(int amount)
        {
            List<Message> messages = _unitOfWork.Message.GetTop(amount);
            List<ThreadView> replies = new List<ThreadView>();
            
            foreach(Message msg in messages)
            {
                replies.Add(new ThreadView()
                {
                    Author = $"{msg.Author.FirstName} {msg.Author.LastName}",
                    AuthorId = msg.Author.Id.ToString(),
                    Content = msg.Content,
                    Id = msg.Id,
                    Parent = msg.Parent?.Id.ToString(),
                    Title = msg.Title,
                    Created = msg.Created,
                    Pinned = msg.Pinned,
                    Guest = _unitOfWork.Message.IsGuest(msg.Id.ToString()),
                    Replies = _unitOfWork.Message.GetReplyCount(msg.Id.ToString())
                });
            }
            
            return replies;
        }

        public List<ThreadView> GetMessageReplies(string id)
        {
            List<Message> messages = _unitOfWork.Message.GetReplies(id);
            List<ThreadView> replies = new List<ThreadView>();
            
            foreach(Message msg in messages)
            {
                replies.Add(new ThreadView()
                {
                    Author = $"{msg.Author.FirstName} {msg.Author.LastName}",
                    AuthorId = msg.Author.Id.ToString(),
                    Content = msg.Content,
                    Id = msg.Id,
                    Parent = msg.Parent?.Id.ToString(),
                    Created = msg.Created,
                    Pinned = msg.Pinned,
                    Guest = _unitOfWork.Message.IsGuest(msg.Id.ToString()),
                    ReplyContent = _unitOfWork.Message.GetReplyChildren(msg.Id.ToString()),
                    Replies = _unitOfWork.Message.GetReplyCount(msg.Id.ToString())
                });
            }
            
            return replies;
        }

        public List<ThreadView> GetShadowBannedMessages()
        {
            List<Message> messages = _unitOfWork.Message.GetShadowBannedMessages();
            List<ThreadView> threadViews = new List<ThreadView>();

            foreach(Message msg in messages)
            {
                threadViews.Add(new ThreadView()
                {
                    Author = $"{msg.Author.FirstName} {msg.Author.LastName}",
                    Title = msg.Title,
                    AuthorId = msg.Author.Id.ToString(),
                    Content = msg.Content,
                    Id = msg.Id,
                    Parent = msg.Parent?.Id.ToString(),
                    Created = msg.Created,
                    Pinned = msg.Pinned,
                    Guest = _unitOfWork.Message.IsGuest(msg.Id.ToString()),
                    ReplyContent = _unitOfWork.Message.GetReplyChildren(msg.Id.ToString()),
                    Replies = _unitOfWork.Message.GetReplyCount(msg.Id.ToString())
                });
            }

            return threadViews;
        }


        //TODO
        public void UpdateBannedMessage(NewBannedMessage newBannedMessage)
        {
            Message message = _unitOfWork.Message.GetById(newBannedMessage.PostId);
            if(message != null)
            {
                if (!newBannedMessage.Banned)
                {
                    if(message.Parent != null)
                    {
                        message.Type = MessageType.Reply;
                    }
                    else
                    {
                        message.Type = MessageType.Thread;
                    }
                }
                else
                {
                    RemoveBannedMessage(message);

                    //ban user ofzo
                }
                _unitOfWork.SaveChanges();
            }
        }

        public void RemoveBannedMessage(Message message)
        {
            foreach (Report report in _unitOfWork.Report.GetByPostId(message.Id))
            {
                _unitOfWork.Report.Remove(report);
            }
            foreach (Message msg in _unitOfWork.Message.GetReplies(message.Id.ToString()))
            {
                RemoveBannedMessage(msg);
            }
            _unitOfWork.Message.Remove(message);
        }
    }
}