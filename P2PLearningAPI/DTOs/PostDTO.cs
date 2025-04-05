using P2PLearningAPI.Interfaces;
using P2PLearningAPI.Models;

namespace P2PLearningAPI.DTOs
{
    public class PostDTO
    {
        public required string Title { get; set; }
        public required string Content { get; set; }
        public required string PostedBy { get; set; }
        public long? DiscussionId { get; set; } = null;
        public long? QuestionId { get; set; } = null;
        public long? AnswerId { get; set; } = null;
        public PostType PostType { get; set; } // Question, Answer, Reply
    }
}
