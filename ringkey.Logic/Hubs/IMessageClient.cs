using System.Collections.Generic;
using System.Threading.Tasks;
using ringkey.Common.Models.Messages;

namespace ringkey.Logic.Hubs
{
    public interface IMessageClient
    {
        Task SendMessages(List<Message> message);
        Task SendMessage(IMessage message);
    }
}