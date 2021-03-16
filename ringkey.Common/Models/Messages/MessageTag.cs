using System;
using System.ComponentModel.DataAnnotations;

namespace ringkey.Common.Models.Messages
{
    public class MessageTag
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Message Message { get; set; }
        public MessageTagType Type { get; set; }
        public string Icon { get; set; }
    }
}