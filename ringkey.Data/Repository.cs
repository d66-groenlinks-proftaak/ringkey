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
        
        public Repository()
        {
            _connection = RethinkDB.R
                .Connection()
                .Hostname("127.0.0.1")
                .Port(28015)
                .Timeout(60)
                .Connect();

            if (!RethinkDB.R.DbList().Contains("ringkey").Run(_connection))
            {
                RethinkDB.R.DbCreate("ringkey").Run(_connection);
                RethinkDB.R.Db("ringkey").TableCreate("Account").Run(_connection);
                RethinkDB.R.Db("ringkey").TableCreate("Role").Run(_connection);
                RethinkDB.R.Db("ringkey").TableCreate("Permission").Run(_connection);
                RethinkDB.R.Db("ringkey").TableCreate("Message").Run(_connection);
            }
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