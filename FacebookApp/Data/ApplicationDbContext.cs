using System;
using System.Collections.Generic;
using System.Text;
using FacebookApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FacebookApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, Role, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserHasFriend>()
                .HasKey(u => new { u.FriendId, u.UserId });

            builder.Entity<UserLikesPost>()
                .HasKey(u => new { u.UserId, u.PostId });

        }

        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<UserHasFriend> UserHasFriends { get; set; }
        public virtual DbSet<UserLikesPost> UserLikesPosts { get; set; }
        public virtual DbSet<UserCommentsOnPost> UserCommentsOnPosts { get; set; }
    }
}
