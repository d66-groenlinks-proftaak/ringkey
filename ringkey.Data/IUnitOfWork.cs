﻿using System;
using ringkey.Data.Accounts;
using ringkey.Data.BannedWords;
using ringkey.Data.Messages;
using ringkey.Data.Reports;

namespace ringkey.Data
{
    public interface IUnitOfWork
    {
        IAccountRepository Account { get; }
        IMessageRepository Message { get; }
        IBannedWordsRepository BannedWords { get; }
        IReportRepository Report { get; }
    }
}