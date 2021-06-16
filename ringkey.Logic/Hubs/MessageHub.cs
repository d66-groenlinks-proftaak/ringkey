using Microsoft.AspNetCore.SignalR;
using ringkey.Common.Models;

using ringkey.Common.Models.Messages;

using ringkey.Common.Models.Accounts;
using ringkey.Common.Models.Roles;

using ringkey.Data;
using ringkey.Logic;
using ringkey.Logic.Accounts;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using ringkey.Logic.Messages;
using Utility = ringkey.Logic.Accounts.Utility;


namespace ringkey.Logic.Hubs
{
    public partial class MessageHub : Hub<IMessageClient>
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

        public async Task RequestSortedList(MessageSortType type)
        {
            await Clients.Caller.SendThreads(_messageService.GetLatest("Alle berichten", 10, type));

        }

        public async Task RequestUpdate(string tag)
        {
            await Clients.Caller.SendThreads(_messageService.GetLatest(tag, 10));
        }

        public async Task RequestAnnouncement()
        {
            List<ThreadView> announcements = _messageService.GetAnnouncements();
            
            await Clients.Caller.SendAnnouncements(announcements);
        }

        public async Task ReportMessage(NewReport newReport) // ur reported dude
        {
            if (Context.Items.ContainsKey("account"))
            {
                Account account = (Account) Context.Items["account"];
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

        public async Task EditRole(NewRole newRole)
        {
            List<Permission> perms = new List<Permission>();
            if (newRole.Permissions != null)
            {
                foreach (NewPermission perm in newRole.Permissions)
                {
                    perms.Add(new Permission()
                    {
                        Perm = (Permissions) perm.Code
                    });
                }
            }

            if (_unitOfWork.Role.GetByName(newRole.Name) != null)
            {
                Role role = _unitOfWork.Role.GetByName(newRole.Name);
                _unitOfWork.Permission.RemoveExistingPermissions(role);
                role.Permissions = perms;
                _unitOfWork.SaveChanges();
                await Clients.Caller.ConfirmRoleEdit(true);
            }
            else
                await Clients.Caller.ConfirmRoleEdit(false);
        }

        public async Task CreateRole(NewRole newRole)
        {
            List<Permission> perms = new List<Permission>();
            if (newRole.Permissions != null)
            {
                foreach (NewPermission perm in newRole.Permissions)
                {
                    perms.Add(new Permission()
                    {
                        Perm = (Permissions) perm.Code
                    });
                }
            }

            if (newRole.Name.Length <= 2)
                await Clients.Caller.ConfirmRoleCreation(RoleCreationError.NameTooShort);
            else if (newRole.Name.Length >= 20)
                await Clients.Caller.ConfirmRoleCreation(RoleCreationError.NameTooLong);
            else if (_unitOfWork.Role.GetByName(newRole.Name) != null)
                await Clients.Caller.ConfirmRoleCreation(RoleCreationError.NameTaken);
            else
            {
                Role role = new Role()
                {
                    Name = newRole.Name,
                    Permissions = perms
                };
                _unitOfWork.Role.Add(role);
                _unitOfWork.SaveChanges();
                await Clients.Caller.ConfirmRoleCreation(RoleCreationError.Success);
            }
        }

        public async Task GetRoleList()
        {
            List<Role> roles = _unitOfWork.Role.GetAllRoles();

            await Clients.Caller.ReceiveRoleList(roles);
        }

        public async Task GetLatestPoll()
        {
            Account _account = new();
            if (Context.Items.ContainsKey("account"))
            {
                Account account = (Account) Context.Items["account"];
                _account = _unitOfWork.Account.GetById(account.Id.ToString());
            }
            else
            {
                await Clients.Caller.ReceivePollResults(_unitOfWork.Poll.GetPollResults());
                return;
            }

            if (!_unitOfWork.Poll.CheckIfVoted(_account))
                await Clients.Caller.ReceiveLatestPoll(_unitOfWork.Poll.GetPollToSend());
            else
                await Clients.Caller.ReceivePollResults(_unitOfWork.Poll.GetPollResults());
        }

        public async Task VoteOnPoll(NewVoteOptions voteOptions)
        {
            Account _account = new();
            if (Context.Items.ContainsKey("account"))
            {
                Account account = (Account) Context.Items["account"];
                 _account = _unitOfWork.Account.GetById(account.Id.ToString());
            }
            
            _unitOfWork.Poll.VotePoll(_account, voteOptions);
            _unitOfWork.SaveChanges();
            await Clients.Caller.ReceivePollResults(_unitOfWork.Poll.GetPollResults());
        }

        public async Task CreateReply(NewReply message)
        {
            MessageErrors error;
            if (Context.Items.ContainsKey("account") && _unitOfWork.Message.GetById(message.Parent).locked == false)
                error = _messageService.CreateReply(message, (Account) Context.Items["account"]);
            else
                error = _messageService.CreateReply(message, null);

            if (error != MessageErrors.NoError)
                await Clients.Caller.MessageCreationError(error);
        }

        public async Task CreatePoll(NewPoll newPoll)
        {
            Poll poll = _unitOfWork.Poll.AddNewPoll(newPoll);
            _unitOfWork.SaveChanges();
            if (poll != null)
                await Clients.Caller.ConfirmPollCreation(true);
            else
                await Clients.Caller.ConfirmPollCreation(false);
        }

        public async Task GetShadowBannedMessages()
        {
            await Clients.Caller.SendShadowBannedMessages(_messageService.GetShadowBannedMessages());
        }

        public async Task LoadMessageThread(string id)
        {
            Message message = _messageService.GetMessageDetails(id);
            if (Context.Items.ContainsKey("account"))
            {
                Account account = (Account)Context.Items["account"];
                Account newAccount = _unitOfWork.Account.GetById(account.Id.ToString());
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
                        Created = message.Created,
                        Attachments = message.Attachments,
                        Locked = message.locked,
                        Rating = (_unitOfWork.Message.GetMessageRating(message.Id.ToString(), MessageRatingType.liked).Count - _unitOfWork.Message.GetMessageRating(message.Id.ToString(), MessageRatingType.disliked).Count),
                        UserRating = (int)_unitOfWork.Message.GetMessageRatingById(message.Id.ToString(), newAccount).Type
            },
                    Children = _messageService.GetMessageReplies(id)
                });
            }
            else
            {
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
                        Created = message.Created,
                        Attachments = message.Attachments,
                        Locked = message.locked,
                        Rating = message.getRatingCount(),
                        Webinar = message.Webinar

                    },
                    Children = _messageService.GetMessageReplies(id)
                });
            }
        }


        public async Task EditMessage(NewEditMessage newEditMessage)
        {
            _messageService.EditMessage(newEditMessage);
        }

        public async Task UpdateBannedMessages(NewBannedMessage newBannedMessage)

        {
            _messageService.UpdateBannedMessage(newBannedMessage);
            if (newBannedMessage.Banned)
            {
                await Clients.Caller.ConfirmBannedMessageUpdate(BannedMessageConfirmation.MessageDeleted);
            }
            else
            {
                await Clients.Caller.ConfirmBannedMessageUpdate(BannedMessageConfirmation.MessageAdded);
            }

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

        public class UpdateRole
        {
            public string Role { get; set; }
            public bool State { get; set; }
            public string Email { get; set; }
        }
        
        public async Task SetRole(UpdateRole r)
        {
            Account a = _unitOfWork.Account.GetByEmail(r.Email);
            
            if (!r.State)
            {
                a.Roles.Remove(a.Roles.Find(ro => ro.Name == r.Role));
            }
            else if(a.Roles.Find(ro => ro.Name == r.Email) == null)
            {
                a.Roles.Add(_unitOfWork.Role.GetByName(r.Role));
            }
            
            _unitOfWork.SaveChanges();
        }

        public async Task UpdateProfile(UpdateProfile updates)
        {
            Account _account = new();
            if (Context.Items.ContainsKey("account"))
            {
                Account account = (Account) Context.Items["account"];
                _account = _unitOfWork.Account.GetById(account.Id.ToString());
            }
            else
            {
                return;
            }

            _account.Biography = updates.Biography;
            _account.ProfilePicture = updates.Avatar;
            
            _unitOfWork.SaveChanges();
        }

        public async Task TogglePostPin(string postId)
        {
            _unitOfWork.Message.PinMessage(postId);
        }

        public async Task LockPost(string postId)
        {
                _unitOfWork.Message.LockMessage(postId);
        }

        public async Task GetCategories()
        {
            await Clients.Caller.SendCategories(_unitOfWork.Category.GetCategories());
        }

        public async Task DeleteCategory(string name)
        {
            _unitOfWork.Category.Remove(name);
            await Clients.Caller.SendCategories(_unitOfWork.Category.GetCategories());
        }

        public async Task AddCategory(string name, string icon)
        {
            _unitOfWork.Category.AddCategory(name, icon);
            await Clients.Caller.SendCategories(_unitOfWork.Category.GetCategories());
        }

        public async Task HideCategory(string name, bool hidden)
        {
            _unitOfWork.Category.HideCategory(name, hidden);
            await Clients.Caller.SendCategories(_unitOfWork.Category.GetCategories());
        }

        public async Task SetAnnouncement(string postId)
        {
            Message message = _unitOfWork.Message.GetById(postId);
            foreach(MessageTag messageTag in message.Tags)
            {
                if(messageTag.Type != MessageTagType.Announcement)
                {
                    _unitOfWork.Message.RemoveAnnouncement(postId);
                }
            }
            
            _unitOfWork.Message.AddAnnouncement(postId);
        }

        public async Task SetRating(string postId, string userEmail, int type)
        {
            Account _account = _unitOfWork.Account.GetByEmail(userEmail);
            if (_account != null)
            {

                MessageRating rating = _unitOfWork.Message.GetMessageRatingById(postId, _account);
                if (rating.Account == null)
                {
                    _unitOfWork.Message.CreateNewRating(postId, (MessageRatingType)type, _account);
                }
                else if (rating.Type == (MessageRatingType)type)
                {
                    _unitOfWork.Message.removeRating(postId, _account);

                }
                else if (rating.Type != (MessageRatingType)type)
                {
                    _unitOfWork.Message.removeRating(postId, _account);
                    _unitOfWork.Message.CreateNewRating(postId, (MessageRatingType)type, _account);
                }
            }
        }
    }
}