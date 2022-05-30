using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistance
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Company> Companies { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder) 
            => base.OnModelCreating(modelBuilder);
    }
}