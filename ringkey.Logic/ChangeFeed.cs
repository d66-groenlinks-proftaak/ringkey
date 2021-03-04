using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ringkey.Common.Models;
using ringkey.Data;
using ringkey.Logic.Hubs;

namespace ringkey.Logic
{
    public class ChangeFeed : BackgroundService
    {
        private UnitOfWork _unitOfWork;
        private readonly IServiceScopeFactory _factory;
        private Timer _timer;
        
        private readonly IHubContext<MessageHub, IMessageClient> _messageHub;
        public ChangeFeed(IServiceScopeFactory factory, IHubContext<MessageHub, IMessageClient> messageHub)
        {
            _factory = factory;
            _messageHub = messageHub;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            using (IServiceScope scope = _factory.CreateScope())
            {
                _unitOfWork = scope.ServiceProvider.GetRequiredService<UnitOfWork>();
            }
            
            var cursor = await _unitOfWork.Message.MessageChange();

            while (await cursor.MoveNextAsync())
            {
                if (cursor.Current != null && cursor.Current.OldValue == null)
                {
                    Console.WriteLine("Testing!");
                    await _messageHub.Clients.All.SendMessage(cursor.Current.NewValue);
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            
        }
    }
}