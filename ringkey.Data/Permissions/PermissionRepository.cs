using ringkey.Common.Models;
using ringkey.Common.Models.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ringkey.Data.Permissions
{
    public class PermissionRepository : Repository<Permission>, IPermissionRepository
    {
        public void RemoveExistingPermissions(Role role)
        {
            foreach (Permission permission in role.Permissions)
            {
                _dbContext.Permission.Remove(permission);
            }
        }
    
        public PermissionRepository(RingkeyDbContext context) : base(context)
        {
        }
    }
}
