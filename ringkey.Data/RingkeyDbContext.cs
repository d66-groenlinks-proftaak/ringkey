using Microsoft.EntityFrameworkCore;
using ringkey.Common.Models;
using ringkey.Common.Models.Messages;

namespace ringkey.Data
{
    public class RingkeyDbContext : DbContext
    {
        public DbSet<Message> Message { get; set; }
        
        public RingkeyDbContext(DbContextOptions options) : base(options) {}
    }
}