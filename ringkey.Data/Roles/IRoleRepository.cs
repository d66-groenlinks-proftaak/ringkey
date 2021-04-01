using ringkey.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ringkey.Data.Roles
{
    public interface IRoleRepository : IRepository<Role>
    {
        Role GetByName(string name);
        List<Role> GetAllRoles();
    } 
}
