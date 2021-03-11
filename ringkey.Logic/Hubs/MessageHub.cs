using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using ringkey.Common.Models.Messages;
using ringkey.Common.Models;
using ringkey.Data;
using ringkey.Logic.Messages;

namespace ringkey.Logic.Hubs
{
    public class MessageHub : Hub<IMessageClient>
    {
        private UnitOfWork _unitOfWork;
        private MessageService _messageService;
        
        public MessageHub(UnitOfWork unitOfWork, MessageService messageService)
        {
            _unitOfWork = unitOfWork;
            _messageService = messageService;
        }

        public override Task OnConnectedAsync()
        {
            Context.Items["page"] = "/";

            Groups.AddToGroupAsync(Context.ConnectionId, "/");
            
            return base.OnConnectedAsync();
        }

        public async Task RequestUpdate()
        {
            await Clients.Caller.SendMessages(_unitOfWork.Message.GetLatest(10));
        }

        public async Task UpdatePage(string page)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, (string) Context.Items["page"] ?? string.Empty);
            await Groups.AddToGroupAsync(Context.ConnectionId, $"{page}");
            
            Context.Items["page"] = $"{page}";
        }

        public async Task CreateMessage(NewMessage message)
        {
            _messageService.CreateMessage(message);
        }

        public async Task ReportMessage(Report report)
        {
            await Clients.Caller.ConfirmReport(true);
        }

        public async Task CreateReply(NewReply message)
        {
            _messageService.CreateReply(message);
        }
        
        public async Task LoadMessageThread(string id)
        {
            await Clients.Caller.SendThreadDetails(new Thread()
            {
                Parent = _messageService.GetMessageDetails(id),
                Children = _messageService.GetMessageReplies(id)
            });
        }


        /// <summary>
        /// Retrieve profile data from database
        /// </summary>
        /// <param name="id">id attached with the account (in url)</param>
        /// <returns>the profile data to display in the browser</returns>
        public async Task GetProfile(string id)
        {
            List<Dictionary<string, string>> profiles = new List<Dictionary<string, string>>();

            #region testing profiles - to be removed
            Dictionary<string, string> profile1Data = new Dictionary<string, string>() {
                { "id", "1" },
                { "firstname", "" },
                { "lastname", "Koppejan" },
                { "email", "martijn.koppejan1@gmail.com" }
            };

            Dictionary<string, string> profile2Data = new Dictionary<string, string>() {
                { "id", "asdf" },
                { "firstname", "a" },
                { "lastname", "bob" },
                { "email", "xxx@xxx" }
            };

            profiles.Add(profile1Data);
            profiles.Add(profile2Data);
            #endregion

            Dictionary<string, string> temp = null;
            foreach (Dictionary<string, string> profile in profiles)
            {
                if (profile["id"].Equals(id)) temp = profile;
            }

            await Clients.Caller.SendProfile(temp);
        }
    }
}