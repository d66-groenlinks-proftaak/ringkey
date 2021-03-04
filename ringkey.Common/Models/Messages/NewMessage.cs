using System;

namespace ringkey.Common.Models.Messages
{
    public class NewMessage : IMessage
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime Created { get; set; }
        public string Email { get; set; }
        public string Author { get; set; }
    }
}