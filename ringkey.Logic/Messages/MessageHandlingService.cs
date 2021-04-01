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
using ringkey.Common.Models.Accounts;
using ringkey.Logic.Roles;

namespace ringkey.Logic.Messages
{
    public class MessageHandlingService : BackgroundService
    {
        private UnitOfWork _unitOfWork;
        private IHubContext<MessageHub, IMessageClient> _hub;
        private IServiceScopeFactory _services;
        private List<string> _bannedWords;

        private CharCombination[] _charCombinations = { new CharCombination('e', '3'), new CharCombination('a', '4'), new CharCombination('i', '1'), new CharCombination('l', '1'), new CharCombination('o', '0'), new CharCombination('b', '8'), new CharCombination('g', '9'), new CharCombination('a', '@'), new CharCombination('a', '*'), new CharCombination('e', '*'), new CharCombination('o', '*'), new CharCombination('u', '*'), new CharCombination('i', '*')};

        public MessageHandlingService(IServiceScopeFactory services)
        {
            _services = services;
        }
        
        public Message GetTopParent(string id)
        {
            Message msg = _unitOfWork.Message.GetById(id, false);

            if (msg.Parent == null)
                return msg;

            return GetTopParent(msg.Parent.Id.ToString());
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            using (var scope = _services.CreateScope())
            {
                _unitOfWork = scope.ServiceProvider.GetService<UnitOfWork>();
                List<BannedWord> tempbannedWords = _unitOfWork.BannedWords.GetAllBannedWords();
                _bannedWords = new List<string>();
                foreach(BannedWord bannedWord in tempbannedWords)
                {
                    _bannedWords.AddRange(leetSpeak(bannedWord.Word));
                }
            }

            while (!cancellationToken.IsCancellationRequested)
            {
                using (var scope = _services.CreateScope()) {
                    _unitOfWork = scope.ServiceProvider.GetService<UnitOfWork>();
                    _hub = scope.ServiceProvider.GetService<IHubContext<MessageHub, IMessageClient>>();

                    List<Message> ToFilterMessages = _unitOfWork.Message.GetUnprocessed();

                    foreach (Message message in ToFilterMessages.ToList())
                    {
                        bool res = messageConatinsBannedWord(message);
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
                                Parent =  message.Parent?.Id.ToString(),
                                Title = message.Title,
                                Created = message.Created,
                                Guest = _unitOfWork.Message.IsGuest(message.Id.ToString()),
                            });
                        else if (message.Type == MessageType.Reply)
                        {
                            string top = GetTopParent(message.Id.ToString()).Id.ToString();
                            
                            await _hub.Clients.Group($"/thread/{GetTopParent(message.Id.ToString()).Id.ToString()}")
                                .SendChild(new ThreadView()
                                {
                                    Author = $"{message.Author.FirstName} {message.Author.LastName}",
                                    AuthorId = message.Author.Id.ToString(),
                                    Content = message.Content,
                                    Id = message.Id,
                                    Parent = message.Parent?.Id.ToString(),
                                    Created = message.Created,
                                    Guest = _unitOfWork.Message.IsGuest(message.Id.ToString()),
                                });
                            ;
                        }
                    }

                    _unitOfWork.SaveChanges();
                }
                await Task.Delay(1000);
            }
        }

        private bool messageConatinsBannedWord(Message message)
        {
            if (message.Title != null && containsBannedWords(message.Title))
                return true;
            if (message.Content != null && containsBannedWords(message.Content))
                return true;

            return false;
        }

        private bool containsBannedWords(string input)
        {
            string words = input.Replace(" ", "");
            words = words.Replace("_", "");
            words = words.Replace("-", "");

            foreach (string bannedWord in _bannedWords)
            {
                if (words.ToLower().Contains(bannedWord.ToLower()))
                    return true;
            }

            return false;
        }

        private List<string> leetSpeak(string input)
        {
            List<string> output = new List<string>();
            output.Add(input);

            foreach (CharCombination charCombination in _charCombinations)
            {
                char[] letterArray = input.ToCharArray();
                for (int i = 0; i < letterArray.Length; i++)
                {
                    if (letterArray[i] == charCombination.Value)
                    {
                        letterArray[i] = charCombination.ReplaceValue;
                        if (output.Find(x => x.Contains(new string(letterArray))) == null)
                        {
                            output.Add(new string(letterArray));
                            List<string> templist = leetSpeak(new string(letterArray));
                            foreach (string tempval in templist)
                            {
                                if (output.Find(x => x.Contains(tempval)) == null)
                                    output.Add(tempval);
                            }
                        }
                        letterArray[i] = charCombination.Value;
                    }
                }
            }
            return output;
        }
    }
}
