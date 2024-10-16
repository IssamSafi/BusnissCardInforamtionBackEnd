using Microsoft.EntityFrameworkCore;
using System;

namespace BusnissCardInforamtion.Model
{
    public class BusnissCarddbContext : DbContext
    {
        public BusnissCarddbContext(DbContextOptions options) : base(options) { }
        public DbSet<BusinessCard> busnissCards { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BusinessCard>().Property(b => b.PhotoBase64)
                .HasMaxLength(1048576); // Example for max length if needed
        }
    }
}
