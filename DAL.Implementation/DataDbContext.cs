using Microsoft.EntityFrameworkCore;
using Models;

namespace DAL.Implementation
{
    public class DataDbContext : DbContext
    {
        public DataDbContext(DbContextOptions<DataDbContext> options) : base(options) { }

        public DbSet<Tree> Trees { get; set; }

        public DbSet<Node> Nodes { get; set; }

        public DbSet<JournalItem> JournalItems { get; set; }
    }
}
