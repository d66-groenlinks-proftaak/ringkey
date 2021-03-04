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
            message.Created = DateTime.Now.ToUniversalTime();
            
            _unitOfWork.Message.Create(message);
            
            return true;
        }
    }
}