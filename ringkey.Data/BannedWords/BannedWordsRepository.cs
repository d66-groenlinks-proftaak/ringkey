using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ringkey.Common.Models;

namespace ringkey.Data.BannedWords
{
    public class BannedWordsRepository : Repository<BannedWord>, IBannedWordsRepository
    {
        public List<BannedWord> GetAllBannedWords()
        {
            return _dbContext.BannedWords.ToList();
        }


        public BannedWordsRepository(RingkeyDbContext context) : base(context)
        {

        }
    }
}
