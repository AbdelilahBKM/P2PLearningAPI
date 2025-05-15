namespace P2PLearningAPI.DTOoutput
{
    public class JoiningsDTOoutput
    {
        public long Id { get; set; }
        public string UserId { get; set; }
        public string PostId { get; set; }
        public DateTime JoinedAt { get; set; }

        public JoiningsDTOoutput() {
            Id=0;
            JoinedAt = DateTime.Now;
        }
        public JoiningsDTOoutput(long id, string userId, string postId, DateTime joinedAt)
        {
            Id = id;
            UserId = userId;
            PostId = postId;
            JoinedAt = joinedAt;
        }
    }
}
