using System;
using System.Collections.Generic;
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
       
        public Message CreateMessage(NewMessage message)
        {
            Console.WriteLine(Utility.CheckMessage(message));
            if (Utility.CheckMessage(message) == MessageErrors.NoError)
            {
                Message newMessage = new Message()
                {
                    Author = message.Author,
                    Content = message.Content,
                    Created = DateTimeOffset.Now.ToUnixTimeMilliseconds(),
                    Type = MessageType.Thread,
                    Title = message.Title,
                    Processed = false
                };

                _unitOfWork.Message.Add(newMessage);

                _unitOfWork.SaveChanges();

                return newMessage;
            }
            return new Message();
        }

        public Message GetMessageDetails(string id)
        {
            return _unitOfWork.Message.GetById(id);
        }

        public Message CreateReply(NewReply reply)
        {
            Message newMessage = new Message()
            {
                Author = reply.Author,
                Content = reply.Content,
                Created = DateTimeOffset.Now.ToUnixTimeMilliseconds(),
                Parent = reply.Parent,
                Type = MessageType.Reply,
                Processed = false
            };
            
            _unitOfWork.Message.Add(newMessage);
            
            _unitOfWork.SaveChanges();

            return newMessage;
            
            _unitOfWork.SaveChanges();
        }
        
        public List<Message> GetMessageReplies(string id)
        {
            return _unitOfWork.Message.GetReplies(id);
        }
    }
}