using Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class DataContext : IdentityDbContext<AppUser>
    {
        public DataContext(DbContextOptions options) : base(options) { }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<ActivityAttendee> ActivityAttendees { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<UserFollowing> UserFollowings { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ActivityAttendee>().HasKey(x => new {x.ActivityId, x.AppUserId});
            builder.Entity<ActivityAttendee>()
                .HasOne(u => u.AppUser)
                .WithMany(a => a.Activities)
                .HasForeignKey(x => x.AppUserId);
            builder.Entity<ActivityAttendee>()
                .HasOne(u => u.Activity)
                .WithMany(a => a.Attendees)
                .HasForeignKey(x => x.ActivityId);
            
            builder.Entity<Comment>()
                .HasOne(x => x.Activity)
                .WithMany(a => a.Comments)
                .OnDelete(DeleteBehavior.Cascade);
            
            builder.Entity<UserFollowing>(x => {
                x.HasKey(a => new {a.ObserverId, a.TargetId});
                
                x.HasOne(a => a.Observer)
                    .WithMany(b => b.Followings)
                    .HasForeignKey(c => c.ObserverId)
                    .OnDelete(DeleteBehavior.Cascade);

                x.HasOne(a => a.Target)
                    .WithMany(b => b.Followers)
                    .HasForeignKey(c => c.TargetId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

        }
    }
}