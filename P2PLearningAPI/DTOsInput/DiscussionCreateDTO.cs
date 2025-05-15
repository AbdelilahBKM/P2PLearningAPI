using P2PLearningAPI.Models;

namespace P2PLearningAPI.DTOs
{
    public class CreateDiscussionDTO
    {
        public required string d_Name { get; set; } = string.Empty;
        public required string d_Description { get; set; } = string.Empty;
        public string? d_Profile { get; set; } = string.Empty;
        public required string OwnerId { get; set; }
    }
}
