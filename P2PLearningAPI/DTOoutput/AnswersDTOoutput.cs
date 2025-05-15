namespace P2PLearningAPI.DTOoutput
{
    public class AnswersDTOoutput
    {
        public long Id { get; set; }
        public string Content { get; set; } = null!;
        public string PostedBy { get; set; } = null!;
        public DateTime Created_at { get; set; }
        public DateTime Updated_at { get; set; }
        public long QuestionId { get; set; }
        public int VotesCount { get; set; } = 0;
        public bool isAccepted { get; set; } = false;
        public AnswersDTOoutput()
        {
            Created_at = DateTime.Now;
            Updated_at = DateTime.Now;
        }
        public AnswersDTOoutput(long id, string content, string postedBy, DateTime createdAt, DateTime updatedAt, long questionId)
        {
            Id = id;
            Content = content;
            PostedBy = postedBy;
            Created_at = createdAt;
            Updated_at = updatedAt;
            QuestionId = questionId;
        }
    }
}
