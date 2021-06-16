using System.Collections.Generic;
using System.Threading.Tasks;
using ringkey.Common.Models;
using ringkey.Common.Models.Messages;
using ringkey.Common.Models.Roles;

namespace ringkey.Logic.Hubs
{
    public interface IMessageClient
    {
        Task SendAnnouncements(List<ThreadView> message);
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

        Task SendShadowBannedMessages(List<ThreadView> messages);
        Task ConfirmRoleCreation(RoleCreationError error);
        Task ReceiveRoleList(List<Role> roleList);
        Task ConfirmBannedMessageUpdate(BannedMessageConfirmation confirmation);
        Task ConfirmRoleEdit(bool success);
        Task ConfirmPollCreation(bool success);
        Task ReceiveLatestPoll(PollToSend poll);
        Task ReceivePollResults(PollResults results);
        Task SendCategories(List<Category> categories);
    }
}