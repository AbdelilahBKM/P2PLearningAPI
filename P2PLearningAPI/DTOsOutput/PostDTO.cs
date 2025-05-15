namespace P2PLearningAPI.DTOsOutput
{
    public class PostDTO
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public long Reputation { get; set; }
        public DateTime PostedAt { get; set; }
        public bool IsClosed { get; set; }

        public UserMiniDTO PostedBy { get; set; }
    }

}
