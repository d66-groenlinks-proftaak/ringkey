using System.Collections.Generic;
using ringkey.Common.Models;

namespace ringkey.Data.Polls
{
    public class PollRepository : Repository<Poll>, IPollRepository
    {
        public Poll AddNewPoll(NewPoll newPoll)
        {
            List<PollOption> options = new();
            foreach (string option in newPoll.PollOptions)
            {
                options.Add(new PollOption()
                {
                    Name = option,
                    VoteCount = 0
                });
            }
            Poll poll = new()
            {
                Options = options,
                Name = newPoll.PollName,
                MultipleOptions = newPoll.MultipleOptions,
                ExpirationDate = newPoll.ExpirationDate
            };
            _dbContext.Add(poll);
            return poll;
        }
        
        public PollRepository(RingkeyDbContext context) : base(context)
        {
        }
    }
}