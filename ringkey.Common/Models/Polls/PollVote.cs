using System;
using System.ComponentModel.DataAnnotations;

namespace ringkey.Common.Models
{
    public class PollVote
    {
        [Key]
        public Guid Id { get; set; }
        public Account Account { get; set; }
        public PollOption PollOption { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}