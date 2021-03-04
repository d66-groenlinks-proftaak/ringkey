using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ringkey.Common.Models;
using ringkey.Data;

namespace ringkey.Logic
{
    public class ChangeFeed : IHostedService
    {
        private UnitOfWork _unitOfWork;
        private readonly IServiceScopeFactory _factory;
        private Timer _timer;
        
        public ChangeFeed(IServiceScopeFactory factory)
        {
            _factory = factory;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using (IServiceScope scope = _factory.CreateScope())
            {
                _unitOfWork = scope.ServiceProvider.GetRequiredService<UnitOfWork>();
            }
            
            var cursor = await _unitOfWork.Account.AccountChange();

            while (await cursor.MoveNextAsync())
            {
                if (cursor.Current != null)
                {
                    Console.WriteLine($"Update: {cursor.Current.NewValue.id}; credits: {cursor.Current.NewValue.Credits}");
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer.Change(Timeout.Infinite, 0);
            
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            
        }
    }
}