using System;
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

        public bool CreateMessage(NewMessage message)
        {
            _unitOfWork.Message.Add(new Message()
            {
                id = Guid.NewGuid().ToString(),
                Author = message.Author,
                Content = message.Content,
                Created = DateTimeOffset.Now.ToUnixTimeMilliseconds(),
                Title = message.Title
            });
            
            _unitOfWork.SaveChanges();
            
            return true;
        }
    }
}