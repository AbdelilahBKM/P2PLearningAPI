namespace P2PLearningAPI.Models
{
    public class Badge
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        // Navigation property for the many-to-many relationship
        public List<UserBadge> UserBadges { get; set; } = new List<UserBadge>();
    }
}

