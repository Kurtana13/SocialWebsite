using Microsoft.EntityFrameworkCore;
using SocialWebsite.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

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

            builder.Entity<User>()
                .HasMany<Group>(u => u.Groups)
                .WithMany(g => g.Users)
                .UsingEntity(
                l => l.HasOne(typeof(User)).WithMany().HasForeignKey("UserId"),
                r => r.HasOne(typeof(Group)).WithMany().HasForeignKey("GroupId"))
                .ToTable("UserGroup");
        }

        public DbSet<Comment> Comments { get; set; }
        public DbSet<Friend> Friends { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<User> Users {  get; set; }
    }
}
