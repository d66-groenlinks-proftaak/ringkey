using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ringkey.Common.Models.Messages;
using ringkey.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ringkey.Logic.Messages
{
    public class MessageHandlingService : BackgroundService
    {
        private UnitOfWork _unitOfWork;
        private IServiceScopeFactory _services;

        public MessageHandlingService(IServiceScopeFactory services)
        {
            _services = services;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("test1");
            
            while (!cancellationToken.IsCancellationRequested)
            {
                using (var scope = _services.CreateScope()) {
                    _unitOfWork = scope.ServiceProvider.GetService<UnitOfWork>();
                }
                
                Console.WriteLine("test2");
                await Task.Delay(1000);
                Console.WriteLine("test3");
                List<Message> ToFilterMessages = _unitOfWork.Message.GetUnprocessed();
                Console.WriteLine(ToFilterMessages.Count);
                
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
                if (word.ToLower().Contains("gaming"))
                    return true;
            }
            return false;
        }
    }
}
