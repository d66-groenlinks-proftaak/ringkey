using System;
using ringkey.Data.Accounts;
using ringkey.Data.Messages;

namespace ringkey.Data
{
    public interface IUnitOfWork
    {
        IAccountRepository Account { get; }
        IMessageRepository Message { get; }
    }
}