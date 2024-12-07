using P2PLearningAPI.Models;

namespace P2PLearningAPI.DTOs
{
    public class DiscussionDTO
    {
        public required string d_Name { get; set; } = string.Empty;
        public string? d_Profile { get; set; } = string.Empty;
        public required User Owner { get; set; }
    }
}
