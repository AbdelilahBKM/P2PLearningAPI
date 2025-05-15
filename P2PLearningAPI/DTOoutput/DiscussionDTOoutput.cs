namespace P2PLearningAPI.DTOoutput
{
    public class DiscussionDTOoutput
    {
        public long Id { get; set; }
        public string D_Name { get; set; }
        public string D_Profile { get; set; }
        public string D_Description { get; set; } = string.Empty;
        public int Number_of_members { get; set; } = 0;
        public int Number_of_active_members { get; set; } = 0;
        public string OwnerId { get; set; }
        public DateTime Created_at { get; set; } = DateTime.Now;
        public DateTime Updated_at { get; set; } = DateTime.Now;
    }
}
