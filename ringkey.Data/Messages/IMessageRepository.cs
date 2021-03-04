using System.Collections.Generic;
using System.Threading.Tasks;
using RethinkDb.Driver.Model;
using RethinkDb.Driver.Net;
using ringkey.Common.Models;
using ringkey.Common.Models.Messages;

namespace ringkey.Data.Messages
{
    public interface IMessageRepository : IRepository<IMessage>
    {
        Task<Cursor<Change<Message>>> MessageChange();
        void Create(IMessage message);
        List<Message> GetLatest(int amount);
    }
}