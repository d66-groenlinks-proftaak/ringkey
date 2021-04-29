using System;
using System.Collections.Generic;

namespace ringkey.Common.Models
{
    public class NewPoll
    {
        public string PollName { get; set; }
        public List<string> PollOptions { get; set; }
        public bool MultipleOptions { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}