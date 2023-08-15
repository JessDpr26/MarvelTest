using MarvelTest.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Abstractions;

namespace MarvelTest.Data
{
    public class MarvelDBContext : DbContext
    {
        public MarvelDBContext(DbContextOptions<MarvelDBContext> options) : base(options)
        {
        }

        public DbSet<Characters> Characters { get; set; }
        public DbSet<Comics> Comics { get; set; }
        public DbSet<LogEntry> LogEntries { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<LogEntry>().HasNoKey(); 
        }

    }
}
