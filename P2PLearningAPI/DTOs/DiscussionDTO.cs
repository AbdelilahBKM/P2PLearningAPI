namespace P2PLearningAPI.DTOs
{
    public class DiscussionDTO
    {
        public int Id { get; set; }
        public string d_Name { get; set; } = string.Empty;
        public string d_Profile { get; set; } = string.Empty;
        public int NumberOfMembers { get; set; }
        public int NumberOfActiveMembers { get; set; }
        public int NumberOfPosts { get; set; }
        public int OwnerId { get; set; }
        public string Owner { get; set; } = string.Empty;
    }
}
