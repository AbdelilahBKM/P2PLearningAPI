using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
namespace P2PLearningAPI.Models
{
    public class P2PLearningDbContext: DbContext
    {
        public P2PLearningDbContext(DbContextOptions<P2PLearningDbContext> options): base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Scholar> Scholars { get; set; }
        public DbSet<Administrator> Administrators { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<Discussion> Discussions { get; set; }
        public DbSet<Joining> Joinings { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Vote> Votes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //==> Primary keys
            modelBuilder.Entity<User>().HasKey(x => x.Id);
            modelBuilder.Entity<Scholar>().HasKey(x => x.Id);
            modelBuilder.Entity<Administrator>().HasKey(x => x.Id);
            modelBuilder.Entity<Request>().HasKey(x => x.Id);
            modelBuilder.Entity<Discussion>().HasKey(x => x.Id);
            modelBuilder.Entity<Joining>().HasKey(x => x.Id);
            modelBuilder.Entity<Post>().HasKey(x => x.Id);
            modelBuilder.Entity<Question>().HasKey(x => x.Id);
            modelBuilder.Entity<Answer>().HasKey(x => x.Id);
            modelBuilder.Entity<Vote>().HasKey(x => x.Id);

            //==> Constraints

            // User => Admin & Scholar
            modelBuilder.Entity<Scholar>()
                .HasOne(s => s.User)
                .WithMany()
                .HasForeignKey(s => s.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction); 

            modelBuilder.Entity<Administrator>()
                .HasOne(a => a.User)
                .WithMany()
                .HasForeignKey(a => a.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            // Scholar has many requests
            modelBuilder.Entity<Scholar>()
                .HasMany<Request>(s => s.Requests)
                .WithOne(r => r.User)
                .HasForeignKey(r => r.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            // Post => Question & Answer
            modelBuilder.Entity<Question>()
                .HasOne<Post>(q => q.Post)
                .WithMany()
                .HasForeignKey(q => q.PostId)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction); 

            modelBuilder.Entity<Answer>()
                .HasOne<Post>(a => a.Post)
                .WithMany()
                .HasForeignKey(a => a.PostId)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            // Question has many Answers
            modelBuilder.Entity<Question>()
                .HasMany<Answer>(q => q.Answers)
                .WithOne(a => a.Question)
                .HasForeignKey(a => a.QuestionId)
                .OnDelete(DeleteBehavior.NoAction);

            // Discussion has many Questions
            modelBuilder.Entity<Discussion>()
                .HasMany<Question>(d => d.Questions)
                .WithOne(q => q.Discussion)
                .HasForeignKey(d => d.DiscussionId)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            // Joining => User & Discussion
            modelBuilder.Entity<Joining>()
                .HasOne<User>(j => j.User)
                .WithMany(u => u.Joinings)
                .HasForeignKey(j => j.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Joining>()
                .HasOne<Discussion>(j => j.Discussion)
                .WithMany(d => d.Joinings)
                .HasForeignKey(j => j.DiscussionId)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            // Votes => User & Post
            modelBuilder.Entity<Vote>()
                .HasOne<User>(v => v.User)
                .WithMany(u => u.Votes)
                .HasForeignKey(v => v.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Vote>()
                .HasOne<Post>(v => v.Post)
                .WithMany(p => p.Votes)
                .HasForeignKey(v => v.PostId)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            // User has many Posts
            modelBuilder.Entity<Post>()
                .HasOne<User>(p => p.PostedBy)
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

        }

    }
}
