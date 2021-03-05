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
            message.Created = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            
            _unitOfWork.Message.Create(message);
            _unitOfWork.SaveChanges();
            
            return true;
        }
    }
}