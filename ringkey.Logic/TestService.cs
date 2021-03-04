using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RethinkDb.Driver.Model;
using ringkey.Common.Models;
using ringkey.Data;

namespace ringkey.Logic
{
    public class TestService : IHostedService, IDisposable
    {
        private UnitOfWork _unitOfWork;
        private readonly IServiceScopeFactory _factory;
        private Timer _timer;
        
        public TestService(IServiceScopeFactory factory)
        {
            _factory = factory;
        }

        private async void DoWork(object? state)
        {
            Account account = _unitOfWork.Account.Get("00d20e9a-4a73-4c74-af00-57ea97c8aaa9");
            account.Credits = account.Credits + 1;
            _unitOfWork.Account.Update(account);
            
            Console.WriteLine("Account updated!");
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            using (IServiceScope scope = _factory.CreateScope())
            {
                _unitOfWork = scope.ServiceProvider.GetRequiredService<UnitOfWork>();
            }

            return Task.CompletedTask;
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