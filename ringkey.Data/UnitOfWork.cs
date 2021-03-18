﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ringkey.Data.Accounts;
using ringkey.Data.BannedWords;
using ringkey.Data.Messages;
using ringkey.Data.Reports;

namespace ringkey.Data
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        public IAccountRepository Account { get; private set; }
        public IMessageRepository Message { get; private set; }
        public IBannedWordsRepository BannedWords { get; private set; }
        public IReportRepository Report { get; private set; }

        private RingkeyDbContext _context;

        public UnitOfWork(RingkeyDbContext context)
        {
            _context = context;
            
            Account = new AccountRepository(_context);
            Message = new MessageRepository(_context);
            BannedWords = new BannedWordsRepository(_context);
            Report = new ReportRepository(_context);
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