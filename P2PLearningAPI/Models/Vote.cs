namespace P2PLearningAPI.Models
{
    public enum VoteType
    {
        Positive,
        Negative
    }
    public class Vote
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public User User { get; set; } = null!;
        public long PostId { get; set; }
        public Post Post { get; set; } = null!;
        public VoteType VoteType { get; set; }
        public Vote() { }
        public Vote(User user, Post post, VoteType voteType) { 
            this.User = user;
            this.UserId = user.Id;
            this.Post = post;
            this.PostId = post.Id;
            this.VoteType = voteType;
        }
    }
}
