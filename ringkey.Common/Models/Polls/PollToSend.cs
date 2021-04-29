using System;
using System.Collections.Generic;

namespace ringkey.Common.Models
{
    public class PollToSend
    {
        public Guid Id { get; set; } 
        public string Name { get; set; }
        public bool MultipleOptions { get; set; }
        public List<PollOption> Options { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}