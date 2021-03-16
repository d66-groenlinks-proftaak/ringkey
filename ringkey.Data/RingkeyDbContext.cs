using Microsoft.EntityFrameworkCore;
using ringkey.Common.Models;
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
        public RingkeyDbContext(DbContextOptions options) : base(options) {}
    }
}