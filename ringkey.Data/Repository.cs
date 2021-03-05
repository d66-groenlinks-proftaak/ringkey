﻿using System;
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
        protected IRethinkContext _rethinkContext;
        
        public Repository(IRethinkContext rethinkContext)
        {
            _connection = rethinkContext.Connection;
            _rethinkContext = rethinkContext;
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
            _rethinkContext.AddCommand(() =>
                RethinkDB.R.Db("ringkey").Table(typeof(TEntity).Name.Split(".")[^1]).Insert(entity)
                    .RunAsync(_connection)
            );
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
            _rethinkContext.AddCommand(() =>
                RethinkDB.R.Db("ringkey").Table(typeof(TEntity).Name.Split(".")[^1]).Insert(entities).RunAsync(_connection)
            );
        }
    }
}