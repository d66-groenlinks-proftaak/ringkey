using System;
using System.Collections.Generic;

namespace ringkey.Common.Models.Messages
{
    public class NewMessage : INewMessage
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public long Created { get; set; }
        public string Email { get; set; }
        public string Author { get; set; }
        public string Token { get; set; }
        public List<string> Categories { get; set; }
        public bool Announcement { get; set; }
        public bool Webinar { get; set; }
    }
}