using System;

namespace ringkey.Common.Models.Messages
{
    public class Message
    {
        public string id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Content { get; set; }
        public long Created { get; set; }
    }
}