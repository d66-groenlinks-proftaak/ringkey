using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ringkey.Common.Models;
using ringkey.Common.Models.Accounts;
using ringkey.Data;

namespace ringkey.Logic.Roles
{
    public class RoleStartup : IHostedService
    {
        private UnitOfWork _unitOfWork;
        private IServiceScopeFactory _services;
        
        public RoleStartup(IServiceScopeFactory services)
        {
            _services = services;
        }
        
        public Task StartAsync(CancellationToken cancellationToken)
        {
            using (var scope = _services.CreateScope())
            {
                _unitOfWork = scope.ServiceProvider.GetService<UnitOfWork>();
                CheckAllDefaultRoles();
                _unitOfWork.SaveChanges();
                return Task.CompletedTask;
            }
        }
        
        private void CheckAllDefaultRoles()
        {
            if (_unitOfWork.Role.GetByName("Admin") == null)
            {
                AddAdmin();
            }
            if (_unitOfWork.Role.GetByName("Guest") == null)
            {
                AddGuest();
            }
            if (_unitOfWork.Role.GetByName("Member") == null)
            {
                AddMember();
            }
        }
        private void AddAdmin()
        {
            _unitOfWork.Role.Add(new Role()
            {
                Name = "Admin",
                Permissions = new List<Permission>()
                {
                    new Permission()
                    {
                        Perm = Permissions.Ban
                    },
                    new Permission()
                    {
                        Perm = Permissions.Categorize
                    },
                    new Permission()
                    {
                        Perm = Permissions.Announcement
                    },
                    new Permission()
                    {
                        Perm = Permissions.Webinar
                    },
                    new Permission()
                    {
                        Perm = Permissions.Quiz
                    }
                }
            });
        }

        private void AddGuest()
        {
            _unitOfWork.Role.Add(new Role()
            {
                Name = "Guest",
                Permissions = new List<Permission>()
                {

                }
            });
        }

        private void AddMember()
        {
            _unitOfWork.Role.Add(new Role()
            {
                Name = "Member",
                Permissions = new List<Permission>()
                {
                  
                }
            });
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}