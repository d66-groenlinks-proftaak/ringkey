using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
namespace ringkey.Common.Models.Messages
{
    public partial class Message
    {
        [Key] public Guid Id { get; set; }
        public string Title { get; set; }
        public Account Author { get; set; }
        public string Content { get; set; }
        public MessageType Type { get; set; }
        public Message Parent { get; set; }
        public long Created { get; set; }
        public bool Processed { get; set; }
        public bool Pinned { get; set; }
        public bool Webinar { get; set; }
        public bool locked { get; set; }
        public bool Announcement { get; set; }
        public int Views { get; set; }
        public List<Message> Children { get; set; } = new();
        public List<MessageTag> Tags { get; set; } 
        public List<Report> Reports { get; set; }
        public List<Attachment> Attachments { get; set; }


        // Displayed on home page
        public ThreadView GetThreadView() 
        {
            return new ThreadView()
                {
                    Author = $"{Author.FirstName} {Author.LastName}",
                    AuthorId = Author.Id.ToString(),
                    Content = Content,
                    Id = Id,
                    Parent = Parent?.Id.ToString(),
                    Title = Title,
                    Created = Created,
                    Pinned = Pinned,
                    Guest =  Author.Roles.Any(e => e.Name == "Guest"),
                    Replies = Children.Count(),
                    Role = Author.Roles.First().Name,
                    ReplyContent = Children.Take(3).Select(m => m.GetThreadView()).ToList(),
                    Webinar = Webinar
                };
        }

        // Displayed in thread as replies
        public ThreadView GetAsReply()
        {
            return new ThreadView()
                {
                    Author = $"{Author.FirstName} {Author.LastName}",
                    AuthorId = Author.Id.ToString(),
                    Content = Content,
                    Id = Id,
                    Parent = Parent?.Id.ToString(),
                    Title = Title,
                    Created = Created,
                    Pinned = Pinned,
                    Guest =  Author.Roles.Any(e => e.Name == "Guest")
                };
        }
    }
}