using P2PLearningAPI.Models;

namespace P2PLearningAPI.DTOs
{
    public class VoteDTO
    {
        public string UserId { get; set; }
        public long PostId { get; set; }
        public VoteType VoteType { get; set; }

    }
}
