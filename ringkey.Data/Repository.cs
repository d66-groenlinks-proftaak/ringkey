using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using RethinkDb.Driver;
using RethinkDb.Driver.Net;
using RethinkDb.Driver.Linq;

namespace ringkey.Data
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected Connection _connection;
        
        public Repository(Connection connection)
        {
            _connection = connection;
        }
        
        public TEntity Get(string id)
        {
            return RethinkDB.R.Db("ringkey").Table(typeof(TEntity).Name.Split(".")[^1]).Get(id).Run<TEntity>(_connection);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return RethinkDB.R.Db("ringkey").Run<TEntity>(_connection);
        }

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> expression)
        {
            return RethinkDB.R.Db("ringkey").Table<TEntity>(typeof(TEntity).Name.Split(".")[^1], _connection)
                .Where(expression)
                .ToList();
        }

        public void Add(TEntity entity)
        {
            RethinkDB.R.Db("ringkey").Table(typeof(TEntity).Name.Split(".")[^1]).Insert(entity).Run(_connection);
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
            RethinkDB.R.Db("ringkey").Table(typeof(TEntity).Name.Split(".")[^1]).Insert(entities).Run(_connection);
        }
    }
}