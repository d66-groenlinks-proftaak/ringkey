using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using ringkey.Common.Models.Messages;
using ringkey.Common.Models;
using ringkey.Data;
using ringkey.Logic.Accounts;
using ringkey.Logic.Messages;

namespace ringkey.Logic.Hubs
{
    public class MessageHub : Hub<IMessageClient>
    {
        private UnitOfWork _unitOfWork;
        private MessageService _messageService;
        private AccountService _accountService;
        
        public MessageHub(UnitOfWork unitOfWork, MessageService messageService, AccountService accountService)
        {
            _unitOfWork = unitOfWork;
            _messageService = messageService;
            _accountService = accountService;
        }

        public override Task OnConnectedAsync()
        {
            Context.Items["page"] = "/";

            Groups.AddToGroupAsync(Context.ConnectionId, "/");
            
            return base.OnConnectedAsync();
        }

        public async Task RequestSortedList(MessageSortType type)
        {
            if(type == MessageSortType.New)
                await Clients.Caller.SendThreads(_messageService.GetLatest(10));
            if(type == MessageSortType.Top)
                await Clients.Caller.SendThreads(_messageService.GetTop(10));
            if(type == MessageSortType.Old)
                await Clients.Caller.SendThreads(_messageService.GetOldest(10));
        }

        public async Task RequestUpdate()
        {
            await Clients.Caller.SendThreads(_messageService.GetLatest(10));
        }

        public async Task UpdatePage(string page)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, (string) Context.Items["page"] ?? string.Empty);
            await Groups.AddToGroupAsync(Context.ConnectionId, $"{page}");
            
            Context.Items["page"] = $"{page}";
        }

        public async Task CreateMessage(NewMessage message)
        {
            MessageErrors error;
            
            if(Context.Items.ContainsKey("account"))
                error = _messageService.CreateMessage(message, (Account)Context.Items["account"]);
            else
                error = _messageService.CreateMessage(message, null);

            Console.WriteLine(error);

            if (error != MessageErrors.NoError)
            {
                Console.WriteLine("Send error!");
                await Clients.Caller.MessageCreationError(error);
            }
        }

        public async Task Authenticate(string token)
        {
            Account acc = _accountService.GetByToken(token);
            if (acc != null)
            {
                await Clients.Caller.Authenticated(new AuthenticateResponse()
                {
                    Email = acc.Email,
                    AccountId = acc.Id.ToString(),
                    Token = Accounts.Utility.GenerateJwtToken(_unitOfWork.Account.GetByEmail(acc.Email))
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
                    Token = Accounts.Utility.GenerateJwtToken(_unitOfWork.Account.GetByEmail(account.Email))
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
                    Token = Accounts.Utility.GenerateJwtToken(acc)
                });
                
                Context.Items["account"] = acc;
            }

        }

        public async Task ReportMessage(Report report)
        {
            await Clients.Caller.ConfirmReport(true);
        }

        public async Task CreateReply(NewReply message)
        {
            MessageErrors error;
            
            if(Context.Items.ContainsKey("account"))
                error = _messageService.CreateReply(message, (Account)Context.Items["account"]);
            else
                error = _messageService.CreateReply(message, null);

            if (error != MessageErrors.NoError)
                await Clients.Caller.MessageCreationError(error);
        }
        
        public async Task LoadMessageThread(string id)
        {
            Message message = _messageService.GetMessageDetails(id);
            
            await Clients.Caller.SendThreadDetails(new Thread()
            {
                Parent = new ThreadView()
                {
                    Author = $"{message.Author.FirstName} {message.Author.LastName}",
                    AuthorId = message.Author.Id.ToString(),
                    Content = message.Content,
                    Id = message.Id,
                    Parent = message.Parent?.Id.ToString(),
                    Title = message.Title,
                    Created = message.Created
                },
                Children = _messageService.GetMessageReplies(id)
            });
        }


        /// <summary>
        /// Retrieve profile data from database
        /// </summary>
        /// <param name="id">id attached with the account (in url)</param>
        /// <returns>the profile data to display in the browser</returns>
        public async Task GetProfile(string id)
        {
            List<Dictionary<string, string>> profiles = new List<Dictionary<string, string>>();

            #region testing profiles - to be removed
            Dictionary<string, string> profile1Data = new Dictionary<string, string>() {
                { "id", "1" },
                { "firstname", "" },
                { "lastname", "Koppejan" },
                { "email", "martijn.koppejan1@gmail.com" }
            };

            Dictionary<string, string> profile2Data = new Dictionary<string, string>() {
                { "id", "asdf" },
                { "firstname", "a" },
                { "lastname", "bob" },
                { "email", "xxx@xxx" }
            };

            profiles.Add(profile1Data);
            profiles.Add(profile2Data);
            #endregion

            Dictionary<string, string> temp = null;
            foreach (Dictionary<string, string> profile in profiles)
            {
                if (profile["id"].Equals(id)) temp = profile;
            }

            await Clients.Caller.SendProfile(temp);
        }
    }
}