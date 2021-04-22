using System.Collections.Generic;
using ringkey.Common.Models;

namespace ringkey.Data.Polls
{
    public interface IPollRepository : IRepository<Poll>
    {
        Poll AddNewPoll(NewPoll newPoll);
    }
}