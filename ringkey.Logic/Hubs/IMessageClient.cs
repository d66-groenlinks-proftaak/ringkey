using System.Collections.Generic;
using System.Threading.Tasks;
using ringkey.Common.Models.Messages;

namespace ringkey.Logic.Hubs
{
    public interface IMessageClient
    {
        Task SendMessages(List<Message> message);
        Task SendMessage(Message message);
        Task SendThreadDetails(Thread thread);
        Task ConfirmReport(bool ReportConfirmation);


        #region TO BE MOVED
        /// <summary>
        /// To be moved to seperate interface, to accompany the future profile hub
        /// Currently here for testing purposes
        /// </summary>
        Task SendProfile(Dictionary<string, string> profile);
        #endregion
    }
}