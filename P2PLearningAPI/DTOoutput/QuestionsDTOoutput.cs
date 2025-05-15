namespace P2PLearningAPI.DTOoutput
{
    public class QuestionsDTOoutput
    {
        public long Id { get; set; }
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public string PostedBy { get; set; } = null!;
        public DateTime Created_at { get; set; }
        public DateTime Updated_at { get; set; }
        public bool isAnswered { get; set; } = false;
        public long DiscussionId { get; set; }
        public int AnswersCount { get; set; } = 0;
        public int VotesCount { get; set; } = 0;
        public QuestionsDTOoutput()
        {
            Created_at = DateTime.Now;
            Updated_at = DateTime.Now;
        }
        public QuestionsDTOoutput(long id, string title, string content, string postedBy, DateTime createdAt, DateTime updatedAt, bool isAnswered, long discussionId)
        {
            Id = id;
            Title = title;
            Content = content;
            PostedBy = postedBy;
            Created_at = createdAt;
            Updated_at = updatedAt;
            this.isAnswered = isAnswered;
            DiscussionId = discussionId;
        }
    }
}
