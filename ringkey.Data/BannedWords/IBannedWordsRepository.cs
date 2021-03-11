using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ringkey.Common.Models;

namespace ringkey.Data.BannedWords
{
    public interface IBannedWordsRepository : IRepository<BannedWord>
    {
        List<BannedWord> GetAllBannedWords();
    }
}
