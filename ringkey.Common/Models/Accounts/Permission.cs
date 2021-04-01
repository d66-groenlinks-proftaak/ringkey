using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ringkey.Common.Models.Accounts
{
    public class Permission
    {
        [Key]
        public Guid Id { get; set; }
        public Permissions Perm {get;set;}
    }

}
