namespace P2PLearningAPI.Models
{
    public class Discussion
    {
        public long Id { get; set; }
        public string D_Name { get; set; }
        public string D_Profile { get; set; }
        public int Number_of_members { get; set; } = 0;
        public int Number_of_active_members { get; set; } = 0;
        public int Number_of_posts { get; set; } = 0;
        public long OwnerId { get; set; }
        public User Owner { get; set; } = null!;
        public bool IsDeleted { get; set; } = false;
        public ICollection<Question> Questions { get; set; } = new List<Question>();
        public ICollection<Joining> Joinings { get; set; } = new HashSet<Joining>();

    }
}
