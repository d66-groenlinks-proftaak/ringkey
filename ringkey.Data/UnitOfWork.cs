using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ringkey.Data.Accounts;
using ringkey.Data.Messages;

namespace ringkey.Data
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        public IAccountRepository Account { get; private set; }
        public IMessageRepository Message { get; private set; }

        private RingkeyDbContext _context;

        public UnitOfWork(RingkeyDbContext context)
        {
            _context = context;
            
            Account = new AccountRepository(_context);
            Message = new MessageRepository(_context);
            
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}