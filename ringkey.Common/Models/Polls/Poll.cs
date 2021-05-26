using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ringkey.Common.Models
{
    public class Poll
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool MultipleOptions { get; set; }
        public List<PollOption> Options { get; set; }
        public DateTime ExpirationDate { get; set; }
        public List<PollVote> Votes { get; set; }
    }
}