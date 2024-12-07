using P2PLearningAPI.Models;

namespace P2PLearningAPI.DTOs
{
    public class VoteDTO
    {
        public User User { get; set; }
        public Post Post { get; set; }
        public VoteType VoteType { get; set; }

    }
}
