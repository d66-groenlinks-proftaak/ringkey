using System;
using System.ComponentModel.DataAnnotations;

namespace ringkey.Common.Models.Messages
{
    public class Attachment
    {
        [Key]
        public Guid Id { get; set; }
        
        public string Name { get; set; }
        public string Type { get; set; }
    }
}