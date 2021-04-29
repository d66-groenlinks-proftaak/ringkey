using System;
using ringkey.Data.Accounts;
using ringkey.Data.BannedWords;
using ringkey.Data.Messages;
using ringkey.Data.Permissions;
using ringkey.Data.Polls;
using ringkey.Data.Reports;
using ringkey.Data.Roles;

namespace ringkey.Data
{
    public interface IUnitOfWork
    {
        IAccountRepository Account { get; }
        IMessageRepository Message { get; }
        IBannedWordsRepository BannedWords { get; }
        IReportRepository Report { get; }
        IRoleRepository Role { get; }
        IPermissionRepository Permission { get; }
        IPollRepository Poll { get; }
    }
}