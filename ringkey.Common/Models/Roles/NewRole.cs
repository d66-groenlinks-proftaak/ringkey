using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ringkey.Common.Models.Roles
{
    public class NewRole
    {
       public string Name { get; set; }
       public List<NewPermission> Permissions { get; set; }
    }
}
