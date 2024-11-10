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


        public void UpVote()
        {
            this.VoteType = VoteType.Positive;
        }

        public void DownVote()
        {
            this.VoteType = VoteType.Negative;
        }
    }
}
