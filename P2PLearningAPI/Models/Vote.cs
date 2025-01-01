namespace P2PLearningAPI.Models
{
    public enum VoteType
    {
        Positive, // 0
        Negative // 1
    }
    public class Vote
    {
        public long Id { get; set; }
        public string UserId { get; set; }
        public User User { get; set; } = null!;
        public long PostId { get; set; }
        public Post Post { get; set; } = null!;
        public VoteType VoteType { get; set; }
        public Vote() { }
        public Vote(string userId, long postId, VoteType voteType) { 
            this.UserId = userId;
            this.PostId = postId;
            this.VoteType = voteType;
        }
    }
}
