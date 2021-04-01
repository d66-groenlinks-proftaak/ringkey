using Microsoft.AspNetCore.SignalR;
using ringkey.Common.Models;
using ringkey.Common.Models.Messages;
using ringkey.Data;
using ringkey.Logic.Accounts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
            if (type == MessageSortType.New)
                await Clients.Caller.SendThreads(_messageService.GetLatest(10));
            if (type == MessageSortType.Top)
                await Clients.Caller.SendThreads(_messageService.GetTop(10));
            if (type == MessageSortType.Old)
                await Clients.Caller.SendThreads(_messageService.GetOldest(10));
        }

        public async Task RequestUpdate()
        {
            await Clients.Caller.SendThreads(_messageService.GetLatest(10));
        }

        public async Task UpdatePage(string page)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, (string)Context.Items["page"] ?? string.Empty);
            await Groups.AddToGroupAsync(Context.ConnectionId, $"{page}");

            Context.Items["page"] = $"{page}";
        }

        public async Task CreateMessage(NewMessage message)
        {
            MessageErrors error;

            if (Context.Items.ContainsKey("account"))
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

        public async Task ReportMessage(NewReport newReport) // ur reported dude
        {
            if (Context.Items.ContainsKey("account"))
            {
                Account account = (Account)Context.Items["account"];
                Account newAccount = _unitOfWork.Account.GetById(account.Id.ToString());

                Report report = new Report()
                {
                    Account = newAccount,
                    Id = Guid.NewGuid(),
                    Message = _unitOfWork.Message.GetById(newReport.MessageId),
                    ReportMessage = newReport.ReportMessage
                };
                _unitOfWork.Report.Add(report);
                _unitOfWork.SaveChanges();
                await Clients.Caller.ConfirmReport(true);
            }
            await Clients.Caller.ConfirmReport(false);
        }

        public async Task CreateReply(NewReply message)
        {
            MessageErrors error;

            if (Context.Items.ContainsKey("account"))
                error = _messageService.CreateReply(message, (Account)Context.Items["account"]);
            else
                error = _messageService.CreateReply(message, null);

            if (error != MessageErrors.NoError)
                await Clients.Caller.MessageCreationError(error);
        }

        public async Task GetShadowBannedMessages()
        {
            await Clients.Caller.SendShadowBannedMessages(_messageService.GetShadowBannedMessages());
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

        public async Task UpdateBannedMessages(NewBannedMessage newBannedMessage) 
        {
            _messageService.UpdateBannedMessage(newBannedMessage);
        }

        public async Task LoadSubReplies(string id)
        {
            List<ThreadView> newReplies = _messageService.GetNextReplies(id);
            await Clients.Caller.SendChildren(newReplies);
        }


        /// <summary>
        /// Retrieve profile data from database
        /// </summary>
        /// <param name="id">id attached with the account (in url)</param>
        /// <returns>the profile data to display in the browser</returns>
        public async Task GetProfile(string id)
        {
            Profile profile = _unitOfWork.Account.GetProfileById(id);

            await Clients.Caller.SendProfile(profile);
        }
    }
}