using P2PLearningAPI.Models;

namespace P2PLearningAPI.DTOsOutput
{
    public class AnswerDTO: PostDTO
    {
        public long? AnswerId { get; set; }
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
            long? AnswerId,
            long? questionId,
            bool isBestAnswer
            ) : base(Id, Title, Content, IsUpdated, Reputation, PostedAt, UpdatedAt, PostedBy, IsClosed, Votes) {
            this.AnswerId = AnswerId;
            this.QuestionId = questionId;
            this.IsBestAnswer = isBestAnswer;
        }

        public static AnswerDTO FromAnswer(Answer answer)
        {
            return new AnswerDTO(
                answer.Id,
                answer.Title,
                answer.Content,
                answer.IsUpdated,
                answer.Reputation,
                answer.PostedAt,
                answer.UpdatedAt,
                new UserMiniDTO
                {
                    Id = answer.PostedBy.Id,
                    UserName = answer.PostedBy.UserName!,
                    ProfilePicture = answer.PostedBy.ProfilePicture
                },
                answer.IsClosed,
                answer.Votes.Select(v => new VoteDTO
                {
                    Id = v.Id,
                    VoteType = v.VoteType,
                    UserId = v.UserId,
                    PostId = v.PostId
                }).ToList(),
                answer.AnswerId,
                answer.QuestionId,
                answer.IsBestAnswer
                );
        }
    }
}
