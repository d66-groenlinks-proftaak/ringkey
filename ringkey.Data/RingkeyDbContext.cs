using Microsoft.EntityFrameworkCore;
using ringkey.Common.Models;
using ringkey.Common.Models.Accounts;
using ringkey.Common.Models.Messages;

namespace ringkey.Data
{
    public class RingkeyDbContext : DbContext
    {
        public DbSet<Message> Message { get; set; }
        public DbSet<Account> Account { get; set; }
        public DbSet<Report> Report { get; set; }
        public DbSet<MessageTag> Tag { get; set; }
        public DbSet<BannedWord> BannedWords { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<Permission> Permission { get; set; }
        public DbSet<Poll> Poll { get; set; }
        public DbSet<MessageRating> MessageRating { get; set; }

        public DbSet<Category> Category { get; set; }
        public RingkeyDbContext(DbContextOptions options) : base(options) {}
        
    }
}