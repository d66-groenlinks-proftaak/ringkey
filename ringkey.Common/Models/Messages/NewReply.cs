﻿namespace ringkey.Common.Models.Messages
{
    public class NewReply : INewMessage
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public long Created { get; set; }
        public string Author { get; set; }
        public string Parent { get; set; }
        public string Email { get; set; }
    }
}