using ringkey.Common.Models.Accounts;
using System.Collections.Generic;

namespace ringkey.Common.Models
{
    public class AuthenticateResponse
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public string AccountId { get; set; }
        public List<Permissions> Permissions { get; set; }
    }
}