namespace P2PLearningAPI.Models
{
    public class Administrator
    {
        public long Id { get; set; }
        public User User { get; set; } = null!;
        public long UserId { get; set; }
        public string token { get; set; } = string.Empty;
    }
}
