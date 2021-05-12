using System.Collections.Generic;

namespace ringkey.Common.Models
{
    public class PollResults
    {
        public string Name { get; set; }
        public List<Vote> Votes { get; set; }
    }
}