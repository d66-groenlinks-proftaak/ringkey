using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ringkey.Common.Models.Messages
{
    public partial class Message
    {
        [Key] public Guid Id { get; set; }
        public string Title { get; set; }
        public Account Author { get; set; }
        public string Content { get; set; }
        public MessageType Type { get; set; }
        public Message Parent { get; set; }
        public long Created { get; set; }
        public bool Processed { get; set; }
        public bool Pinned { get; set; }
        public int Views { get; set; }
        public List<Message> Children { get; set; }
        public List<MessageTag> Tags { get; set; } 
        public List<Report> Reports { get; set; }
        public List<Attachment> Attachments { get; set; }
    }
}