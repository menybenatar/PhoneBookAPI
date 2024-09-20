using Microsoft.EntityFrameworkCore;
using PhoneBookAPI.Models;

namespace PhoneBookAPI.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<ContactModel> Contacts { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ContactModel>().HasKey(c => c.Id);
            base.OnModelCreating(modelBuilder);
        }
    }
}
