using ringkey.Common.Models.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ringkey.Common.Models
{
    public class Profile
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public List<Role> Roles { get; set; }
        public List<Message> Messages { get; set; }
    }
}
