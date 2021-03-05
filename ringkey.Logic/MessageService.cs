using System;
using System.Collections.Generic;
using ringkey.Common.Models.Messages;
using ringkey.Data;

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
            Message newMessage = new Message()
            {
                Author = message.Author,
                Content = message.Content,
                Created = DateTimeOffset.Now.ToUnixTimeMilliseconds(),
                Type = MessageType.Thread,
                Title = message.Title
            };
            
            _unitOfWork.Message.Add(newMessage);
            
            _unitOfWork.SaveChanges();

            return newMessage;
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
                Type = MessageType.Reply
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