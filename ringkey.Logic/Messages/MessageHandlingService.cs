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
    public class MessageHandlingService : IHostedService
    {
        private UnitOfWork _unitOfWork;
        private IServiceScopeFactory _services;

        public List<Message> ToFilterMessages = new List<Message>();

        public MessageHandlingService(IServiceScopeFactory services)
        {
            _services = services;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            using (var scope = _services.CreateScope()) {
                _unitOfWork = scope.ServiceProvider.GetService<UnitOfWork>();
            }

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
