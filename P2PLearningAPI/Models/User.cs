namespace P2PLearningAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime DateJoined { get; set; }

        // List of questions posted by the user
        public List<Question> Questions { get; set; } = new List<Question>();

        // List of answers given by the user
        public List<Answer> Answers { get; set; } = new List<Answer>();

        // List of UserBadge entities associated with the user
        public List<UserBadge> UserBadges { get; set; } = new List<UserBadge>();

        public int Reputation { get; set; } = 0; // This can increase based on activity like answering and upvotes

        public void AddBadge(Badge badge)
        {
            UserBadges.Add(new UserBadge { Badge = badge, UserId = this.Id, BadgeId = badge.Id });
        }

        public void IncreaseReputation(int points)
        {
            Reputation += points;
        }

        public void DecreaseReputation(int points)
        {
            Reputation = Math.Max(0, Reputation - points);
        }
    }
}
