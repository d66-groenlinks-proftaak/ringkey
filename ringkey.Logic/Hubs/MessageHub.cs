using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using ringkey.Common.Models.Messages;
using ringkey.Data;

namespace ringkey.Logic.Hubs
{
    public class MessageHub : Hub<IMessageClient>
    {
        private UnitOfWork _unitOfWork;
        
        public MessageHub(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
        public async Task RequestUpdate()
        {
            await Clients.Caller.SendMessages(_unitOfWork.Message.GetLatest(10));
        }
    }
}