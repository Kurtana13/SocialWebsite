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
                b.HasMany(u => u.Groups)
                .WithMany(g => g.Users)
                .UsingEntity<UserGroup>();

                //User-Post
                b.HasMany(u => u.Posts)
                .WithOne(p => p.User);

                //User-Comment
                b.HasMany(u => u.Comments)
                .WithOne(c => c.User)
                .OnDelete(DeleteBehavior.NoAction);

            });

            builder.Entity<Group>(b =>
            {
                b.HasKey(x => x.Id);

                //Relationships
                //Group-Post
                b.HasMany(g => g.Posts)
                .WithOne(p => p.Group);
            });

            builder.Entity<Post>(b =>
            {
                b.HasKey(x => x.Id);

                //Relationships
                //Post-Comment
                b.HasMany(p => p.Comments)
                .WithOne(c => c.Post)
                .OnDelete(DeleteBehavior.NoAction);
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
                    .WithMany()
                    .HasForeignKey(n => n.UserId)
                    .OnDelete(DeleteBehavior.NoAction);

                // Configure the foreign key for the FriendUser navigation property
                b.HasOne(f => f.FriendUser)
                    .WithMany()
                    .HasForeignKey(n => n.FriendId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            builder.Entity<Notification>(b =>
            {
                // Configure the composite primary key
                b.HasKey(n => new { n.RecipientId, n.SenderId });

                // Configure the foreign key for the Recipient navigation property
                b.HasOne(n => n.Recipient)
                    .WithMany()
                    .HasForeignKey(n => n.RecipientId)
                    .OnDelete(DeleteBehavior.NoAction);

                // Configure the foreign key for the Sender navigation property
                b.HasOne(n => n.Sender)
                    .WithMany()
                    .HasForeignKey(n => n.SenderId)
                    .OnDelete(DeleteBehavior.NoAction);
            });
        }

        public DbSet<Comment> Comments { get; set; }
        public DbSet<Friend> Friends { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
