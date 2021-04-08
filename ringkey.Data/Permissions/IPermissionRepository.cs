using ringkey.Common.Models;
using ringkey.Common.Models.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ringkey.Data.Permissions
{
    public interface IPermissionRepository : IRepository<Permission>
    {
        void RemoveExistingPermissions(Role role);
    }
}
