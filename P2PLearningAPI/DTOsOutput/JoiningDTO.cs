namespace P2PLearningAPI.DTOsOutput
{
    public class JoiningDTO
    {
        public long Id { get; set; }
        public DateTime JoinedAt { get; set; }
        public UserMiniDTO User { get; set; }
    }
}
