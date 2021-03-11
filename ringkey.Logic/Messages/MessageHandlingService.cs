using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ringkey.Common.Models;
using ringkey.Common.Models.Messages;
using ringkey.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using ringkey.Logic.Hubs;
using Thread = ringkey.Common.Models.Messages.Thread;

namespace ringkey.Logic.Messages
{
    public class MessageHandlingService : BackgroundService
    {
        private UnitOfWork _unitOfWork;
        private IHubContext<MessageHub, IMessageClient> _hub;
        private IServiceScopeFactory _services;
        private MessageService _message;
        private List<BannedWord> BannedWords;

        public MessageHandlingService(IServiceScopeFactory services)
        {
            _services = services;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            using (var scope = _services.CreateScope())
            {
                _unitOfWork = scope.ServiceProvider.GetService<UnitOfWork>();
                BannedWords = _unitOfWork.BannedWords.GetAllBannedWords();
            }
            
            while (!cancellationToken.IsCancellationRequested)
            {
                using (var scope = _services.CreateScope()) {
                    _unitOfWork = scope.ServiceProvider.GetService<UnitOfWork>();
                    _hub = scope.ServiceProvider.GetService<IHubContext<MessageHub, IMessageClient>>();
                    _message = scope.ServiceProvider.GetService<MessageService>();

                    List<Message> ToFilterMessages = _unitOfWork.Message.GetUnprocessed();

                    foreach (Message message in ToFilterMessages.ToList())
                    {
                        bool res = MessageConatinsBannedWord(message);
                        if (res)
                            message.Type = MessageType.PossibleSpam;
                        
                        message.Processed = true;

                        if (message.Type == MessageType.Thread)
                            _hub.Clients.Group("/").SendMessage(message);
                        else if (message.Type == MessageType.Reply)
                        {
                            _hub.Clients.Group($"/thread/{message.Parent}").SendThreadDetails(new Thread()
                            {
                                Parent = _message.GetMessageDetails(message.Parent),
                                Children = _message.GetMessageReplies(message.Parent)
                            });
                        }
                    }
                    
                    _unitOfWork.SaveChanges();
                }
                await Task.Delay(1000);
            }
        }

        private bool MessageConatinsBannedWord(Message message)
        {
            if (ContainsBannedWords(message.Title))
                return true;
            if (ContainsBannedWords(message.Content))
                return true;

            return false;
        }

        private bool ContainsBannedWords(string input)
        {
            string[] words = input.Split(" ");

            foreach(string word in words)
            {
                foreach(BannedWord bannedWord in BannedWords)
                {
                    if (word.ToLower().Contains(bannedWord.Word))
                        return true;
                }
            }
            return false;
        }
    }
}
