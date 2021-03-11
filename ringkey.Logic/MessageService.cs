using System;
using System.Collections.Generic;
using System.Linq;
using ringkey.Common.Models;
using ringkey.Common.Models.Messages;
using ringkey.Data;
using ringkey.Logic.Messages;

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
            Console.WriteLine(Utility.CheckMessage(message));
            MessageErrors error = Utility.CheckMessage(message);
            if (error == MessageErrors.NoError)
            {
                Account account = _unitOfWork.Account.GetByEmail(message.Email);

                if (account != null)
                {
                    if (account.Roles.Any(role => role.Type != RoleType.Guest))
                    {
                        if (authenticated != null && authenticated.Id == account.Id)
                        {
                            
                        } else
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
                        account = _unitOfWork.Account.GetById(account.Id.ToString());
                    }
                }
                
                Message newMessage = new Message()
                {
                    Author = account,
                    Content = message.Content,
                    Created = DateTimeOffset.Now.ToUnixTimeMilliseconds(),
                    Type = MessageType.Thread,
                    Title = message.Title,
                    Processed = false
                };

                _unitOfWork.Message.Add(newMessage);

                _unitOfWork.SaveChanges();

                return MessageErrors.NoError;
            }
            
            return error;
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
                if (authenticated == null)
                    return MessageErrors.EmailAlreadyOwned;

                if (account.Id != authenticated.Id)
                {
                    if (!account.Roles.Contains(new Role()
                    {
                        Type = RoleType.Guest
                    }))
                    {
                        return MessageErrors.EmailAlreadyOwned;
                    }
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
            }

            Message newMessage = new Message()
            {
                Author = account,
                Content = message.Content,
                Created = DateTimeOffset.Now.ToUnixTimeMilliseconds(),
                Type = MessageType.Reply,
                Processed = false
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
                    Content = msg.Content,
                    Id = msg.Id,
                    Parent = msg.Parent,
                    Title = msg.Title,
                    Created = msg.Created
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
                    Content = msg.Content,
                    Id = msg.Id,
                    Parent = msg.Parent,
                    Created = msg.Created
                });
            }
            
            return replies;
        }
    }
}