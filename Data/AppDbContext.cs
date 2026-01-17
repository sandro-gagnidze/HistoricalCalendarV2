using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApplication6.Models;

namespace WebApplication6.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<DailyImage> DailyImages { get; set; }
        public DbSet<DailyImageLocalization> DailyImageLocalizations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<DailyImageLocalization>()
                .HasOne(l => l.DailyImage)
                .WithMany(d => d.Localizations)
                .HasForeignKey(l => l.DailyImageId);
        }
    }
}
