using System.Collections.Generic;
using System.Threading.Tasks;
using ringkey.Common.Models;
using ringkey.Common.Models.Messages;
using ringkey.Common.Models.NewFolder;

namespace ringkey.Logic.Hubs
{
    public interface IMessageClient
    {
        Task SendMessages(List<Message> message);
        Task SendMessage(Message message);
        Task SendThreadDetails(Thread thread);
        Task ConfirmReport(bool ReportConfirmation);
        Task SendChild(Message message);
        Task SendProfile(Dictionary<string, string> profile);
        Task AuthenticateFailed(AccountError error);
        Task Authenticated(AuthenticateResponse authenticateResponse);
    }
}