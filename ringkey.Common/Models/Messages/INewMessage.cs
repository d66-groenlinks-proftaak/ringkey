namespace ringkey.Common.Models.Messages
{
    public interface INewMessage
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string Author { get; set; }
        public string Email { get; set; }
    }
}