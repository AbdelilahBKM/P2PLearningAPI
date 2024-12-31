namespace P2PLearningAPI.Models
{
    public class Joining
    {
        public long Id { get; set; }
        public string UserId {  get; set; }
        public User User { get; set; } = null!;
        public long DiscussionId { get; set; }
        public Discussion Discussion { get; set; } = null!;
        public DateTime JoinedAt { get; set; } = DateTime.Now;
        public Joining() { }
        public Joining(string userId, long discussionId)
        {
            this.DiscussionId = discussionId;
            this.UserId = userId;
        }
    }
}
