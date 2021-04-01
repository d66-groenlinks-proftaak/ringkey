using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using ringkey.Common.Models;
using ringkey.Logic.Accounts;

namespace ringkey.Logic.Hubs
{
    public partial class MessageHub
    {
        public async Task Authenticate(string token)
        {
            Account acc = _accountService.GetByToken(token);
            if (acc != null)
            {
                await Clients.Caller.Authenticated(new AuthenticateResponse()
                {
                    Email = acc.Email,
                    AccountId = acc.Id.ToString(),
                    Token = Utility.GenerateJwtToken(_unitOfWork.Account.GetByEmail(acc.Email))
                });

                Context.Items["account"] = acc;
            }
            else
                await Clients.Caller.AuthenticateFailed(AccountError.InvalidLogin);
        }

        public async Task Login(AccountLogin account)
        {
            Account acc = _accountService.Login(account);
            if (acc != null)
            {
                await Clients.Caller.Authenticated(new AuthenticateResponse()
                {
                    Email = account.Email,
                    AccountId = acc.Id.ToString(),
                    Token = Utility.GenerateJwtToken(_unitOfWork.Account.GetByEmail(account.Email))
                });

                Context.Items["account"] = acc;
            }
            else
                await Clients.Caller.AuthenticateFailed(AccountError.InvalidLogin);
        }

        public async Task Register(AccountRegister account)
        {
            AccountError error = _accountService.Register(account);

            if (error != AccountError.NoError)
                await Clients.Caller.AuthenticateFailed(error);
            else
            {
                Account acc = _unitOfWork.Account.GetByEmail(account.Email);
                
                await Clients.Caller.Authenticated(new AuthenticateResponse()
                {
                    Email = account.Email,
                    Token = Utility.GenerateJwtToken(acc)
                });
                
                Context.Items["account"] = acc;
            }

        }
    }
}