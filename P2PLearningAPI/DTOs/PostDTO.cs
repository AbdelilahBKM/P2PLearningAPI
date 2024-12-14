using P2PLearningAPI.Interfaces;
using P2PLearningAPI.Models;

namespace P2PLearningAPI.DTOs
{
    public class PostDTO
    {
        public required string Title { get; set; }
        public required string Content { get; set; }
        public required User PostedBy { get; set; }
        public Discussion? Discussion { get; set; } = null;
        public Question? Question { get; set; } = null;
        public Answer? Answer { get; set; } = null;
        public PostType PostType { get; set; }
        
    }
}
