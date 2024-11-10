using P2PLearningAPI.Models;

namespace P2PLearningAPI.Models
{
    public class Scholar
    {
        public long Id { get; set; }
        public User User { get; set; } = null!;
        public long UserId { get; set; }
        public int Number_of_request { get; set; } = 10;
        public ICollection<Request> Requests { get; } = new List<Request>();
    }
}
