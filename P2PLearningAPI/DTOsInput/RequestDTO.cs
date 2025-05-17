using P2PLearningAPI.Models;

namespace P2PLearningAPI.DTOs
{
    public class RequestDTO
    {
        public User User { get; set; }
        public string Topic { get; set; }
        public string Description { get; set; }

    }
}
