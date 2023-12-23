using Microsoft.EntityFrameworkCore;
using SocialWebsite.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Reflection.Emit;
using System.Net;

namespace SocialWebsite.Api.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext>options) :base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>(b =>
            {
                b.HasKey(x => x.Id);

                //Relationships
                //User-Group
                //b.HasMany(u => u.Groups)
                //.WithMany(g => g.Users)
                //.UsingEntity<UserGroup>(
                //    l => l.HasOne<Group>().WithMany(e=>e.UserGroups).HasForeignKey(e=>e.GroupId).OnDelete(DeleteBehavior.Restrict),
                //    r => r.HasOne<User>().WithMany(e=>e.UserGroups).HasForeignKey(e=>e.UserId).OnDelete(DeleteBehavior.Cascade));

                //User-Post
                b.HasMany(u => u.Posts)
                .WithOne(p => p.User)
                .OnDelete(DeleteBehavior.Cascade);

                //User-Comment
                b.HasMany(u => u.Comments)
                .WithOne(c => c.User)
                .OnDelete(DeleteBehavior.ClientCascade);

            });

            builder.Entity<UserGroup>(b =>
            {
                b.HasKey(ug => new { ug.UserId, ug.GroupId });

                b.HasOne(ug => ug.User)
                 .WithMany(u => u.UserGroups)
                 .HasForeignKey(ug => ug.UserId)
                 .OnDelete(DeleteBehavior.Cascade);

                b.HasOne(ug => ug.Group)
                 .WithMany(g => g.UserGroups)
                 .HasForeignKey(ug => ug.GroupId)
                 .OnDelete(DeleteBehavior.Cascade);


            });

            builder.Entity<Group>(b =>
            {
                b.HasKey(x => x.Id);

                //Relationships
                //Group-Post
                b.HasMany(g => g.Posts)
                .WithOne(p => p.Group)
                .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<Post>(b =>
            {
                b.HasKey(x => x.Id);

                //Relationships
                //Post-Comment
                b.HasMany(p => p.Comments)
                .WithOne(c => c.Post)
                .OnDelete(DeleteBehavior.ClientCascade);
            });

            builder.Entity<Comment>(b =>
            {
                b.HasKey(x => x.Id);
            });

            builder.Entity<Friend>(b =>
            {
                // Configure the composite primary key
                b.HasKey(f => new { f.UserId, f.FriendId });

                // Configure the foreign key for the User navigation property
                b.HasOne(f => f.User)
                    .WithMany(u=>u.Friends)
                    .HasForeignKey(n => n.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Configure the foreign key for the FriendUser navigation property
                b.HasOne(f => f.FriendUser)
                    .WithMany()
                    .HasForeignKey(n => n.FriendId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            builder.Entity<User>().HasData(
                new { Id = 1,UserName = "Shota", Email = "shota@gmail.com", EmailConfirmed = true, AccessFailedCount = 1, LockoutEnabled = true, PhoneNumberConfirmed = true, TwoFactorEnabled = true },
                new { Id = 2, UserName = "Saba", Email = "saba@gmail.com", EmailConfirmed = true, AccessFailedCount = 1, LockoutEnabled = true, PhoneNumberConfirmed = true, TwoFactorEnabled = true });
        }

        public DbSet<Comment> Comments { get; set; }
        public DbSet<Friend> Friends { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
