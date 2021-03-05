using System.Collections.Generic;

namespace ringkey.Common.Models.Messages
{
    public class Thread
    {
        public Message Parent { get; set; }
        public List<Message> Children { get; set; }
    }
}