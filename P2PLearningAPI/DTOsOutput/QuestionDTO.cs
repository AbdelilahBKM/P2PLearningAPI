using P2PLearningAPI.Models;

namespace P2PLearningAPI.DTOsOutput
{
    public class QuestionDTO: PostDTO
    {
        public long DiscussionId { get; set; }
        public ICollection<AnswerDTO> Answers { get; set; } = new List<AnswerDTO>();
        public bool isAnswered { get; set; } = false;
        public QuestionDTO() { }
        public QuestionDTO(
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
            long discussionId,
            ICollection<AnswerDTO> Answers,
            bool isAnswered
            ) : base(Id, Title, Content, IsUpdated, Reputation, PostedAt, UpdatedAt, PostedBy, IsClosed, Votes) {
            this.DiscussionId = discussionId;
            this.Answers = Answers;
            this.isAnswered = isAnswered;
        }

        public static QuestionDTO FromQuestion(Question question)
        {
            return new QuestionDTO(
                question.Id,
                question.Title,
                question.Content,
                question.IsUpdated,
                question.Reputation,
                question.PostedAt,
                question.UpdatedAt,
                question.PostedBy == null ? new UserMiniDTO() : new UserMiniDTO
                {
                    Id = question.PostedBy.Id,
                    UserName = question.PostedBy.UserName!,
                    ProfilePicture = question.PostedBy.ProfilePicture
                },
                question.IsClosed,
                question.Votes?.Select(v => new VoteDTO
                {
                    Id = v.Id,
                    VoteType = v.VoteType,
                    UserId = v.UserId,
                    PostId = v.PostId
                }).ToList() ?? new List<VoteDTO>(),
                question.DiscussionId,
                question.Answers?.Select(a => AnswerDTO.FromAnswer(a)).ToList() ?? new List<AnswerDTO>(),
                question.isAnswered
            );
        }

    }

}
