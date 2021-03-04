using System;
using ringkey.Data.Accounts;

namespace ringkey.Data
{
    public interface IUnitOfWork
    {
        IAccountRepository Account { get; }
    }
}