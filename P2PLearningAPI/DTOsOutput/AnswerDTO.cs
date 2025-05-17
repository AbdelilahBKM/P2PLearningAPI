namespace P2PLearningAPI.DTOsOutput
{
    public class AnswerDTO: PostDTO
    {
        public long? QuestionId { get; set; }
        public bool IsBestAnswer { get; set; } = false;

        public AnswerDTO() { }
        public AnswerDTO(
            long Id,
            string Title,
            string Content,
            bool IsUpdated,
            long Reputation,
            DateTime PostedAt,
            DateTime UpdatedAt,
            UserMiniDTO PostedBy,
            bool IsClosed,
            ICollection<VoteDTO> Votes,
            long? questionId,
            bool isBestAnswer
            ) : base(Id, Title, Content, IsUpdated, Reputation, PostedAt, UpdatedAt, PostedBy, IsClosed, Votes) {
            this.QuestionId = questionId;
            this.IsBestAnswer = isBestAnswer;
        }
    }
}
