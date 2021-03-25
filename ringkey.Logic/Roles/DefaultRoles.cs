using ringkey.Common.Models;
using ringkey.Common.Models.Accounts;
using ringkey.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ringkey.Logic.Roles
{
    public static class DefaultRoles
    {
        public static void CheckAllDefaultRoles(UnitOfWork _unitOfWork)
        {
            if (_unitOfWork.Role.GetByName("Admin") == null)
            {
                AddAdmin(_unitOfWork);
            }
            if (_unitOfWork.Role.GetByName("Guest") == null)
            {
                AddGuest(_unitOfWork);
            }
            if (_unitOfWork.Role.GetByName("Member") == null)
            {
                AddMember(_unitOfWork);
            }
        }
        public static void AddAdmin(UnitOfWork _unitOfWork)
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

        public static void AddGuest(UnitOfWork _unitOfWork)
        {
            _unitOfWork.Role.Add(new Role()
            {
                Name = "Guest",
                Permissions = new List<Permission>()
                {

                }
            });
        }

        public static void AddMember(UnitOfWork _unitOfWork)
        {
            _unitOfWork.Role.Add(new Role()
            {
                Name = "Member",
                Permissions = new List<Permission>()
                {
                  
                }
            });
        }
    }
}
