using Microsoft.EntityFrameworkCore;
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
        public List<Role> GetAllRoles()
        {
            return _dbContext.Role.Include(a => a.Permissions).ToList();
        }
        public RoleRepository(RingkeyDbContext context) : base(context)
        {
        }
    }
}
