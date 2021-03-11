﻿using System.Collections.Generic;
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
        Task SendChild(Message message);
        Task SendProfile(Dictionary<string, string> profile);
    }
}