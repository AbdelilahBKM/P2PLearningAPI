using Microsoft.EntityFrameworkCore;
using P2PLearningAPI.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
namespace P2PLearningAPI.Data
{
    public class P2PLearningDbContext : IdentityDbContext<User>
    {
        public P2PLearningDbContext(DbContextOptions<P2PLearningDbContext> options) : base(options) {}
        //public DbSet<User> Users { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<Discussion> Discussions { get; set; }
        public DbSet<Joining> Joinings { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Vote> Votes { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder   )
        {
            base.OnModelCreating(modelBuilder);
            //==> Primary keys
            modelBuilder.Entity<User>().HasKey(x => x.Id);
            modelBuilder.Entity<Request>().HasKey(x => x.Id);
            modelBuilder.Entity<Discussion>().HasKey(x => x.Id);
            modelBuilder.Entity<Joining>().HasKey(x => x.Id);
            modelBuilder.Entity<Post>().HasKey(x => x.Id);
            modelBuilder.Entity<Vote>().HasKey(x => x.Id);
            modelBuilder.Entity<Notification>().HasKey(x => x.Id);

            //==> Constraints


            // Scholar has many requests
            modelBuilder.Entity<User>()
                .HasMany(u => u.Requests)
                .WithOne(r => r.User)
                .HasForeignKey(r => r.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            // User has many Notifications
            modelBuilder.Entity<User>()
                .HasMany(u => u.Notifications)
                .WithOne(n => n.User)
                .HasForeignKey(n => n.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            // Post => Question & Answer
            modelBuilder.Entity<Post>()
                .HasDiscriminator<String>("PostType")
                .HasValue<Question>("Question")
                .HasValue<Answer>("Answer");

            // Question has many Answers
            modelBuilder.Entity<Question>()
                .HasMany(q => q.Answers)
                .WithOne(a => a.Question)
                .HasForeignKey(a => a.QuestionId)
                .OnDelete(DeleteBehavior.NoAction);

            // Answer has many Replies
            modelBuilder.Entity<Answer>()
                .HasMany(a => a.Replies)
                .WithOne(a => a.AnswerTo)
                .HasForeignKey(a => a.AnswerId)
                .OnDelete(DeleteBehavior.NoAction);
            // Discussion has many Questions
            modelBuilder.Entity<Discussion>()
                .HasMany(d => d.Questions)
                .WithOne(q => q.Discussion)
                .HasForeignKey(d => d.DiscussionId)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            // Joining => User & Discussion
            modelBuilder.Entity<Joining>()
                .HasOne(j => j.User)
                .WithMany(u => u.Joinings)
                .HasForeignKey(j => j.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Joining>()
                .HasOne(j => j.Discussion)
                .WithMany(d => d.Joinings)
                .HasForeignKey(j => j.DiscussionId)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            // Votes => User & Post
            modelBuilder.Entity<Vote>()
                .HasOne(v => v.User)
                .WithMany(u => u.Votes)
                .HasForeignKey(v => v.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Vote>()
                .HasOne(v => v.Post)
                .WithMany(p => p.Votes)
                .HasForeignKey(v => v.PostId)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            // User has many Posts
            modelBuilder.Entity<Post>()
                .HasOne(p => p.PostedBy)
                .WithMany(u => u.Posts)
                .HasForeignKey(p => p.UserID)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);


            //==> Indexes for performance

            modelBuilder.Entity<Joining>()
                .HasIndex(j => j.UserId);

            modelBuilder.Entity<Joining>()
                .HasIndex(j => j.DiscussionId);

            modelBuilder.Entity<Vote>()
                .HasIndex(v => v.UserId);

            modelBuilder.Entity<Vote>()
                .HasIndex(v => v.PostId);
            modelBuilder.Entity<Vote>()
                .HasIndex(v => new { v.UserId, v.PostId })
                .IsUnique(); // Ensure a user can only vote once per post
            modelBuilder.Entity<Vote>()
                .HasIndex(v => v.VoteType);
            modelBuilder.Entity<Notification>()
                .HasIndex(n => n.UserId);
            modelBuilder.Entity<Notification>()
                .HasIndex(n => n.IsRead);
            modelBuilder.Entity<Notification>()
                .HasIndex(n => n.NotificationType);

        }

    }
}
