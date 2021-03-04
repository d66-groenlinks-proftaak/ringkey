using System;

namespace ringkey.Common.Models.Messages
{
    public interface IMessage
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string Content { get; set; }
        public DateTime Created { get; set; }
    }
}