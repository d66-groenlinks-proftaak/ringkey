﻿using Microsoft.Extensions.DependencyInjection;
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

namespace ringkey.Logic.Messages
{
    public class MessageHandlingService : BackgroundService
    {
        private UnitOfWork _unitOfWork;
        private IHubContext<MessageHub, IMessageClient> _hub;
        private IServiceScopeFactory _services;
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

                    List<Message> ToFilterMessages = _unitOfWork.Message.GetUnprocessed();

                    foreach (Message message in ToFilterMessages.ToList())
                    {
                        bool res = MessageConatinsBannedWord(message);
                        if (res)
                            message.Type = MessageType.PossibleSpam;
                        
                        message.Processed = true;

                        if (message.Type == MessageType.Thread)
                            await _hub.Clients.Group("/").SendMessage(new ThreadView()
                            {
                                Author = $"{message.Author.FirstName} {message.Author.LastName}",
                                Content = message.Content,
                                AuthorId = message.Author.Id.ToString(),
                                Id = message.Id,
                                Parent = message.Parent,
                                Title = message.Title,
                                Created = message.Created
                            });
                        else if (message.Type == MessageType.Reply)
                            await _hub.Clients.Group($"/thread/{message.Parent}").SendChild(new ThreadView()
                            {
                                Author = $"{message.Author.FirstName} {message.Author.LastName}",
                                AuthorId = message.Author.Id.ToString(),
                                Content = message.Content,
                                Id = message.Id,
                                Parent = message.Parent,
                                Created = message.Created
                            });;
                    }
                    
                    _unitOfWork.SaveChanges();
                }
                await Task.Delay(1000);
            }
        }

        private bool MessageConatinsBannedWord(Message message)
        {
            if (message.Title != null && ContainsBannedWords(message.Title))
                return true;
            if (message.Content != null && ContainsBannedWords(message.Content))
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
                    if (word.ToLower().Contains(bannedWord.Word.ToLower()))
                        return true;
                }
            }
            return false;
        }
    }
}
