using System.Collections.Generic;
using ringkey.Common.Models;

namespace ringkey.Data.Polls
{
    public interface IPollRepository : IRepository<Poll>
    {
        Poll AddNewPoll(NewPoll newPoll);
        void VotePoll(Account account, NewVoteOptions newVote);
        PollToSend GetPollToSend();
        bool CheckIfVoted(Account account);
        PollResults GetPollResults();
    }
}