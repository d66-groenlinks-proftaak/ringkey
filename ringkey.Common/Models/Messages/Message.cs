using System;

namespace ringkey.Common.Models.Messages
{
    public class Message : IMessage
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string Content { get; set; }
        public long Created { get; set; }
    }
}