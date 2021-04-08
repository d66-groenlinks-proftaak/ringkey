using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ringkey.Common.Models.Roles
{
    public enum RoleCreationError
    {
        Success,
        NameTooShort,
        NameTooLong,
        NameTaken
    }
}
