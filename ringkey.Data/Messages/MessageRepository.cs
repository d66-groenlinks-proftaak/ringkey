using System.Collections.Generic;
using System.Threading.Tasks;
using RethinkDb.Driver;
using RethinkDb.Driver.Model;
using RethinkDb.Driver.Net;
using ringkey.Common.Models;
using ringkey.Common.Models.Messages;

namespace ringkey.Data.Messages
{
    public class MessageRepository : Repository<IMessage>, IMessageRepository
    {
        public async Task<Cursor<Change<Message>>> MessageChange()
        {
            return await RethinkDB.R.Db("ringkey").Table(nameof(Message).Split(".")[^1]).Changes().RunChangesAsync<Message>(_connection);
        }

        public void Create(IMessage message)
        {
            RethinkDB.R.Db("ringkey").Table("Message").Insert(message).Run(_connection);
        }

        public List<Message> GetLatest(int amount)
        {
            return RethinkDB.R.Db("ringkey").Table("Message").OrderBy(RethinkDB.R.Desc("Created")).Limit(10).Run<List<Message>>(_connection);
        }
    }
}