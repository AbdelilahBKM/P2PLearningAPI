using P2PLearningAPI.Models;

namespace P2PLearningAPI.DTOsOutput
{
    public class VoteDTO
    {
        public long Id { get; set; }
        public string UserId { get; set; } = null!;
        public long PostId { get; set; }
        public VoteType VoteType { get; set; }
        public VoteDTO() { }
        public VoteDTO(long Id, string UserId, long PostId, VoteType VoteType, DateTime Created_at, DateTime Updated_at)
        {
            this.Id = Id;
            this.UserId = UserId;
            this.PostId = PostId;
            this.VoteType = VoteType;
        }
    }
}
