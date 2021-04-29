using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using Microsoft.EntityFrameworkCore;
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
                });
            }
            Poll poll = new()
            {
                Options = options,
                Name = newPoll.PollName,
                MultipleOptions = newPoll.MultipleOptions,
                ExpirationDate = newPoll.ExpirationDate,
                Votes = new List<PollVote>()
            };
            _dbContext.Add(poll);
            return poll;
        }

        private Poll GetLatestPoll()
        {
            List<Poll> polls = _dbContext.Poll.Include(e => e.Options).Include(e => e.Votes).ToList();
            Poll latestPoll = polls.Where(i => i.ExpirationDate > DateTime.Now).OrderByDescending(i => i.Id).First();
            return latestPoll;
        }

        public bool CheckIfVoted(Account account)
        {
            Poll poll = GetLatestPoll();
            if(poll.Votes.Any(e => e.Account.Id == account.Id) || account.Roles.Any(role => role.Name == "Guest") || account.Roles == null)
                return true;
            return false;
        }

        public PollResults GetPollResults()
        {
            Poll poll = GetLatestPoll();
            PollResults pollResults = new()
            {
                Name = poll.Name,
                Votes = poll.Votes.GroupBy(e => e.PollOption.Id).Select(vote => new Vote()
                {
                    PollOption = vote.Select(p => p.PollOption).First(),
                    VoteCount = vote.Select(p => p.Id).Distinct().Count()
                }).ToList()
            };

            return pollResults;
        }
        public PollToSend GetPollToSend()
        {
            Poll poll = GetLatestPoll();
            PollToSend pollToSend = new()
            {
                Id = poll.Id,
                Name = poll.Name,
                Options = poll.Options,
                ExpirationDate = poll.ExpirationDate,
                MultipleOptions = poll.MultipleOptions
            };
            return pollToSend;
        }

        public void VotePoll(Account account, NewVoteOptions newVote)
        {
            Poll poll = GetLatestPoll();
            List<PollOption> options = poll.Options;
            foreach (string voteOption in newVote.VoteOptions)
            {
                PollOption option = options.Find(pollOption => pollOption.Id.ToString() == voteOption);
                poll.Votes.Add(new PollVote()
                {
                    Account = account,
                    PollOption = option,
                    TimeStamp = DateTime.Now
                });
            }
        }
        
        public PollRepository(RingkeyDbContext context) : base(context)
        {
        }
    }
}