﻿using System;
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
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<Profile> Profile { get; set; }

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
                .WithOne(a => a.User)
                .HasForeignKey(a => a.UserId);
            modelBuilder.Entity<ApplicationUser>()
                .HasOne(a => a.Profile)
                .WithOne(a => a.User);
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
