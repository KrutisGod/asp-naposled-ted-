using Microsoft.EntityFrameworkCore;
using MaturitaPvaCviceniASP.Models;

namespace MaturitaPvaCviceniASP.Data
{
    public class MaturitaTryContext : DbContext
    {
        public MaturitaTryContext(DbContextOptions<MaturitaTryContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Note> Notes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Note>()
                .HasOne(c => c.Username)
                .WithMany(a => a.Notes);
        }
    }
}
