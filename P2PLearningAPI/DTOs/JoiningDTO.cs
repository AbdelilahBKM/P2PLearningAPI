using P2PLearningAPI.Models;

namespace P2PLearningAPI.DTOs
{
    public class JoiningDTO
    {
        public required User User { get; set; }
        public required Discussion Discussion { get; set; }
    }
}
