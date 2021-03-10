using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using ringkey.Common.Models.Messages;
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
            Message msg = _messageService.CreateMessage(message);

            await Clients.Group($"/").SendMessage(msg);
        }
        
        public async Task CreateReply(NewReply message)
        {
            Message msg = _messageService.CreateReply(message);

            

            await Clients.Group($"/thread/{message.Parent}").SendThreadDetails(new Thread()
            {
                Parent = _messageService.GetMessageDetails(msg.Parent),
                Children = _messageService.GetMessageReplies(msg.Parent)
            });
        }
        
        public async Task LoadMessageThread(string id)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, (string) Context.Items["page"] ?? string.Empty);
            await Groups.AddToGroupAsync(Context.ConnectionId, $"/thread/{id}");

            Context.Items["page"] = $"thread/{id}";
            
            await Clients.Caller.SendThreadDetails(new Thread()
            {
                Parent = _messageService.GetMessageDetails(id),
                Children = _messageService.GetMessageReplies(id)
            });
        }


        #region TO BE MOVED
        /// <summary>
        /// Dedicaded profile hub needs to be created, currently here for testing purposes
        /// </summary>

        public async Task GetProfile()
        {
            Console.WriteLine("test");
            Dictionary<string, string> profileData = new Dictionary<string, string>() {
                { "firstname", "Martijn" },
                { "lastname", "Koppejan" },
                { "email", "martijn.koppejan1@gmail.com" }
            };

            await Clients.Caller.SendProfile(profileData);
        }

        #endregion
    }
}