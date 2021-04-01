using System.Collections.Generic;
using System.Threading.Tasks;
using ringkey.Common.Models;
using ringkey.Common.Models.Messages;

namespace ringkey.Logic.Hubs
{
    public interface IMessageClient
    {
        Task SendThreads(List<ThreadView> message);
        Task SendMessage(ThreadView message);
        Task SendThreadDetails(Thread thread);
        Task ConfirmReport(bool ReportConfirmation);
        Task SendChild(ThreadView message);
        Task SendProfile(Profile profile);
        Task AuthenticateFailed(AccountError error);
        Task Authenticated(AuthenticateResponse authenticateResponse);
        Task MessageCreationError(MessageErrors error);
        Task SendChildren(List<ThreadView> children);
        Task ConfirmRoleCreation(bool RoleConfirmation);
        Task ReceiveRoleList(List<Role> roleList);
    }
}