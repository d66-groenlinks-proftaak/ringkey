using System;
using System.Collections.Generic;
using System.Linq;
using ringkey.Common.Models;
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

        public MessageErrors CreateMessage(NewMessage message, Account authenticated)
        {
            MessageErrors error;
            Account account = _unitOfWork.Account.GetByEmail(message.Email);

            if (account != null)
            {
                error = Messages.Utility.CheckMessage(message, false);
                
                if (account.Roles.Any(role => role.Type != RoleType.Guest))
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
                
                if (authenticated == null)
                {
                    account = new Account()
                    {
                        Email = message.Email,
                        Password = "",
                        FirstName = message.Author,
                        LastName = "",
                        Roles = new List<Role>()
                        {
                            new Role()
                            {
                                Type = RoleType.Guest
                            }
                        }
                    };

                    _unitOfWork.Account.Add(account);
                }
                else
                {
                    account = _unitOfWork.Account.GetById(authenticated.Id.ToString());
                }
            }

            Message newMessage = new Message()
            {
                Author = account,
                Content = message.Content,
                Created = DateTimeOffset.Now.ToUnixTimeMilliseconds(),
                Type = MessageType.Thread,
                Title = message.Title,
                Processed = false,
                Pinned = false
            };

            _unitOfWork.Message.Add(newMessage);

            _unitOfWork.SaveChanges();

            return MessageErrors.NoError;

        }

        public Message GetMessageDetails(string id)
        {
            return _unitOfWork.Message.GetById(id);
        }

        public MessageErrors CreateReply(NewReply message, Account authenticated)
        {
            Account account = _unitOfWork.Account.GetByEmail(message.Email);

            if (account != null)
            {
                if (account.Roles.Any(role => role.Type != RoleType.Guest))
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
                if (authenticated == null)
                {
                    account = new Account()
                    {
                        Email = message.Email,
                        Password = "",
                        FirstName = message.Author,
                        LastName = "",
                        Roles = new List<Role>()
                        {
                            new Role()
                            {
                                Type = RoleType.Guest
                            }
                        }
                    };

                    _unitOfWork.Account.Add(account);
                }
                else
                {
                    account = _unitOfWork.Account.GetById(authenticated.Id.ToString());
                }
            }

            Message newMessage = new Message()
            {
                Author = account,
                Content = message.Content,
                Created = DateTimeOffset.Now.ToUnixTimeMilliseconds(),
                Parent = message.Parent,
                Type = MessageType.Reply,
                Processed = false,
                Pinned = false
            };

            _unitOfWork.Message.Add(newMessage);

            _unitOfWork.SaveChanges();

            return MessageErrors.NoError;
        }

        public List<ThreadView> GetLatest(int amount)
        {
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
                    Parent = msg.Parent,
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
                    Parent = msg.Parent,
                    Created = msg.Created,
                    Pinned = msg.Pinned,
                    Guest = _unitOfWork.Message.IsGuest(msg.Id.ToString()),
                });
            }
            
            return replies;
        }
    }
}