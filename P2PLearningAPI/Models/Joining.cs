namespace P2PLearningAPI.Models
{
    public class Joining
    {
        public long Id { get; set; }
        public long UserId {  get; set; }
        public User User { get; set; } = null!;
        public long DiscussionId { get; set; }
        public Discussion Discussion { get; set; } = null!;
        public DateTime JoinedAt { get; set; } = DateTime.Now;
        public Joining(User user, Discussion discussion)
        {
            this.User = user;
            this.UserId = user.Id;
            this.Discussion = discussion;
            this.DiscussionId = discussion.Id;

        }
    }
}
