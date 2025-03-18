namespace P2PLearningAPI.Models
{
    public class UserBadge
    {
        public long Id { get; set; }
        public Badge Badge { get; set; }
        public long BadgeId { get; set; }
        public User User { get; set; }
        public string UserId { get; set; }
        public DateTime Awarded_at { get; set; }

        public UserBadge() { }
        public UserBadge(Badge badge, User user, DateTime awarded_at)
        {
            Badge = badge;
            User = user;
            Awarded_at = awarded_at;
        }
    }
}
