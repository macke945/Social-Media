using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Social_Media.Data.DataTables;

namespace Social_Media.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Post> Post { get; set; }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<DislikePost> DislikePost { get; set; }
        public DbSet<Comment> Comment { get; set; }
        public DbSet<DislikeComment> DislikeComment { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureApplicationUser(modelBuilder);
            ConfigurePost(modelBuilder);
            ConfigureDislikePost(modelBuilder);
            ConfigureDislikeComment(modelBuilder);
            ConfigureComment(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        private void ConfigureApplicationUser(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationUser>()
                .HasMany(a => a.Posts)
                .WithOne(a => a.User)
                .HasForeignKey(a => a.UserId);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(a => a.Comments)
                .WithOne(a => a.User)
                .HasForeignKey(a => a.UserId);
        }

        private void ConfigurePost(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Post>()
                .HasOne(p => p.User)
                .WithMany()
                .HasForeignKey(p => p.UserId);

            modelBuilder.Entity<Post>()
                .HasMany(p => p.DislikePosts)
                .WithOne(p => p.Post)
                .HasForeignKey(p => p.PostId);

            modelBuilder.Entity<Post>()
                .HasMany(p => p.Comments)
                .WithOne(p => p.Post)
                .HasForeignKey(p => p.PostId);
        }

        private void ConfigureDislikePost(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DislikePost>()
               .HasKey(x => new { x.UserId, x.PostId });
        }

        private void ConfigureDislikeComment(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DislikeComment>()
               .HasKey(x => new { x.UserId, x.CommentId });
        }
        private void ConfigureComment(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Comment>()
                .HasOne(p => p.User)
                .WithMany()
                .HasForeignKey(p => p.UserId);

            modelBuilder.Entity<Comment>()
                .HasMany(p => p.DislikeComments)
                .WithOne(p => p.Comment)
                .HasForeignKey(p => p.CommentId);
        }
    }
}
