namespace P2PLearningAPI.Models
{
    public class Discussion
    {
        public long Id { get; set; }
        public string D_Name { get; set; }
        public string D_Profile { get; set; }
        public string D_Description { get; set; } = string.Empty;
        public int Number_of_members { get; set; } = 0;
        public int Number_of_active_members { get; set; } = 0;
        public int Number_of_posts { get; set; } = 0;
        public string OwnerId { get; set; }
        public User Owner { get; set; } = null!;
        public bool IsDeleted { get; set; } = false;
        public ICollection<Question> Questions { get; set; } = new List<Question>();
        public ICollection<Joining> Joinings { get; set; } = new HashSet<Joining>();
        public DateTime Created_at { get; set; } = DateTime.Now;
        public DateTime Updated_at { get; set; } = DateTime.Now;
        public Discussion() { }
        public Discussion(string userId,string D_Name, string D_Description, string? D_Profile = null) { 
            this.D_Name = D_Name;
            this.D_Description = D_Description;
            this.D_Profile = D_Profile;
            this.OwnerId = userId;
        }

    }
}
