using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ringkey.Common.Models.Messages
{
    public class ThreadView
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        
        public string AuthorId { get; set; }
        public string Content { get; set; }
        public string Parent { get; set; }
        public long Created { get; set; }
        public int Replies { get; set; }
        public bool Pinned { get; set; }
        public bool Locked { get; set; }
        public bool Guest { get; set; }
        public string Role {get; set;}
        public List<ThreadView> ReplyContent { get; set; }
        public List<Attachment> Attachments { get; set; }
        public bool Webinar { get; set; }       
    }
}