namespace P2PLearningAPI.Models
{
    public class Badge
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string badge_image { get; set; }
        public ICollection<UserBadge> UserBadges { get; set; } = new HashSet<UserBadge>();
        public Badge() { }
        public Badge(string Title, string Description, string image)
        {
            this.Title = Title;
            this.Description = Description;
            badge_image = image;
        }
    }
}
