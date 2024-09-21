using Microsoft.EntityFrameworkCore;
using PhoneBookAPI.Models;

namespace PhoneBookAPI.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<ContactEntity> Contacts { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ContactEntity>().HasKey(c => c.Id);
            base.OnModelCreating(modelBuilder);
        }
    }
}
