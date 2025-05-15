namespace P2PLearningAPI.DTOsOutput
{
    public class DiscussionDTO
    {
        public long Id { get; set; }
        public string D_Name { get; set; }
        public string D_Profile { get; set; }
        public string D_Description { get; set; }
        public int Number_of_members { get; set; }
        public int Number_of_active_members { get; set; }
        public int Number_of_posts { get; set; }

        public UserMiniDTO Owner { get; set; }
        public List<QuestionDTO> Questions { get; set; } = new();
    }

}
