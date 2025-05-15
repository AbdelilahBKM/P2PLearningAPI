using P2PLearningAPI.Models;

namespace P2PLearningAPI.DTOs
{
    public class JoiningCreateDTO
    {
        public required string userId { get; set; }
        public required long discussionId { get; set; }
    }
}
