using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ringkey.Data.Accounts;
using ringkey.Data.BannedWords;
using ringkey.Data.Messages;
using ringkey.Data.Permissions;
using ringkey.Data.Polls;
using ringkey.Data.Reports;
using ringkey.Data.Roles;

namespace ringkey.Data
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        public IAccountRepository Account { get; private set; }
        public IMessageRepository Message { get; private set; }
        public IBannedWordsRepository BannedWords { get; private set; }
        public IReportRepository Report { get; private set; }
        public IRoleRepository Role { get; private set; }
        public IPermissionRepository Permission { get; private set; }
        public IPollRepository Poll { get; private set; }
        public ICategoryRepository Category { get; private set; }
        private RingkeyDbContext _context;

        public UnitOfWork(RingkeyDbContext context)
        {
            _context = context;
            
            Account = new AccountRepository(_context);
            Message = new MessageRepository(_context);
            BannedWords = new BannedWordsRepository(_context);
            Report = new ReportRepository(_context);
            Role = new RoleRepository(_context);
            Permission = new PermissionRepository(_context);
            Poll = new PollRepository(_context);
            Category = new CategoryRepository(_context);
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