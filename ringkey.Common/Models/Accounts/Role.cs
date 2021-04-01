using ringkey.Common.Models.Accounts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ringkey.Common.Models
{
    public class Role
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<Account> Account { get; set; }
        public List<Permission> Permissions { get; set; }
    }
}