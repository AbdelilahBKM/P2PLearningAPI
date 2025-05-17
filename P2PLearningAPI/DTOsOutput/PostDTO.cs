namespace P2PLearningAPI.DTOsOutput
{
    public class PostDTO
    {
        public long Id { get; set; }
        public required string Title { get; set; }
        public required string Content { get; set; }
        public bool IsUpdated { get; set; }
        public long Reputation { get; set; }
        public DateTime PostedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public UserMiniDTO PostedBy { get; set; } = null!;
        public bool IsClosed { get; set; }
        public ICollection<VoteDTO> Votes { get; set; } = new HashSet<VoteDTO>();

        public PostDTO() { }
        public PostDTO(
            long Id, 
            string Title, 
            string Content, 
            bool IsUpdated, 
            long Reputation, 
            DateTime PostedAt, 
            DateTime UpdatedAt, 
            UserMiniDTO PostedB,
            bool IsClosed,
            ICollection<VoteDTO> Votes
            )
        {
            this.Id = Id;
            this.Title = Title;
            this.Content = Content;
            this.IsUpdated = IsUpdated;
            this.Reputation = Reputation;
            this.PostedAt = PostedAt;
            this.UpdatedAt = UpdatedAt;
            this.PostedBy = PostedBy;
            this.IsClosed = IsClosed;
            this.Votes = Votes;
        }

    }

}
