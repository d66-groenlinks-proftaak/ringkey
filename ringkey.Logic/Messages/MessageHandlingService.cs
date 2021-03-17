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

namespace ringkey.Logic.Messages
{
    public class MessageHandlingService : BackgroundService
    {
        private UnitOfWork _unitOfWork;
        private IHubContext<MessageHub, IMessageClient> _hub;
        private IServiceScopeFactory _services;
        private List<BannedWord> bannedWords;

        private CharCombination[] charCombinations = { new CharCombination('e', '3'), new CharCombination('a', '4'), new CharCombination('i', '1'), new CharCombination('l', '1'), new CharCombination('o', '0'), new CharCombination('b', '8'), new CharCombination('g', '9'), new CharCombination('a', '@'), new CharCombination('a', '*'), new CharCombination('e', '*'), new CharCombination('o', '*'), new CharCombination('u', '*'), new CharCombination('i', '*'), new CharCombination('i', 'l'), new CharCombination('l', 'i')};

        public MessageHandlingService(IServiceScopeFactory services)
        {
            _services = services;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            using (var scope = _services.CreateScope())
            {
                _unitOfWork = scope.ServiceProvider.GetService<UnitOfWork>();
                bannedWords = _unitOfWork.BannedWords.GetAllBannedWords();
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
                                Parent = message.Parent,
                                Title = message.Title,
                                Created = message.Created,
                                Guest = _unitOfWork.Message.IsGuest(message.Id.ToString()),
                            });
                        else if (message.Type == MessageType.Reply)
                            await _hub.Clients.Group($"/thread/{message.Parent}").SendChild(new ThreadView()
                            {
                                Author = $"{message.Author.FirstName} {message.Author.LastName}",
                                AuthorId = message.Author.Id.ToString(),
                                Content = message.Content,
                                Id = message.Id,
                                Parent = message.Parent,
                                Created = message.Created,
                                Guest = _unitOfWork.Message.IsGuest(message.Id.ToString()),
                            });;
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

            foreach (BannedWord bannedWord in bannedWords)
            {
                List<string> allBannedWords = leetSpeak(bannedWord.Word);
                foreach(string word in allBannedWords)
                {
                    if (words.ToLower().Contains(word.ToLower()))
                        return true;
                }
            }

            return false;
        }

        private List<string> leetSpeak(string input)
        {
            List<string> output = new List<string>();
            output.Add(input);

            foreach (CharCombination combination in charCombinations)
            {
                if (input.Contains(combination.Value))
                {
                    string temp = input.Replace(combination.Value, combination.ReplaceValue);
                    if (output.Find(x => x.Contains(temp)) == null)
                        output.Add(temp);

                    List<string> templist = leetSpeak(temp);
                    foreach (string tempval in templist)
                    {
                        if (output.Find(x => x.Contains(tempval)) == null)
                            output.Add(tempval);
                    }
                }
            }

            return output;
        }
    }
}
