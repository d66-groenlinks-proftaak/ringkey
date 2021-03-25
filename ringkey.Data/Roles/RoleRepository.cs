using ringkey.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ringkey.Data.Roles
{
    public class RoleRepository :  Repository<Role>, IRoleRepository
    {
        public Role GetByName(string name)
        {
            return _dbContext.Role.FirstOrDefault(r => r.Name == name);
        }
        public RoleRepository(RingkeyDbContext context) : base(context)
        {
        }
    }
}
