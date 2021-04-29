using System;
using System.ComponentModel.DataAnnotations;

namespace ringkey.Common.Models
{
    public class PollOption
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}