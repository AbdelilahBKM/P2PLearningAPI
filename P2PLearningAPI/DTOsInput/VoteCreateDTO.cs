using P2PLearningAPI.Models;

namespace P2PLearningAPI.DTOs
{
    public class VoteCreateDTO
    {
        public string UserId { get; set; }
        public long PostId { get; set; }
        public VoteType VoteType { get; set; }

    }
}
