using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Social_Media.Data.DataTables;

namespace Social_Media.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Post> Post { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureApplicationUser(modelBuilder);
            ConfigurePost(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }
        private void ConfigureApplicationUser(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationUser>()
                .HasMany(a => a.Posts)
                .WithOne(c => c.User)
                .HasForeignKey(c => c.UserId);
        }

        private void ConfigurePost(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Post>()
                .HasOne(p => p.User)
                .WithMany()
                .HasForeignKey(p => p.UserId);
        }
    }
}
