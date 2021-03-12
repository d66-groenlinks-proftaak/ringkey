using System.Collections.Generic;

namespace ringkey.Common.Models.Messages
{
    public class Thread
    {
        public ThreadView Parent { get; set; }
        public List<ThreadView> Children { get; set; }
    }
}