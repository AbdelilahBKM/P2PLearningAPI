using P2PLearningAPI.Models;

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
        public string OwnerId { get; set; }
        public bool IsDeleted { get; set; } = false;
        public ICollection<QuestionDTO> Questions { get; set; } = new List<QuestionDTO>();
        public ICollection<JoiningDTO> Joinings { get; set; } = new HashSet<JoiningDTO>();
        public DateTime Created_at { get; set; } = DateTime.Now;
        public DateTime Updated_at { get; set; } = DateTime.Now;

        public DiscussionDTO() { }

        public DiscussionDTO(
            long Id, 
            string D_Name, 
            string D_Profile, 
            string D_Description, 
            int Number_of_members, 
            int Number_of_active_members, 
            int Number_of_posts, 
            string OwnerId, 
            bool IsDeleted, 
            ICollection<QuestionDTO> Questions, 
            ICollection<JoiningDTO> Joinings, 
            DateTime Created_at, 
            DateTime Updated_at
        )
        {
            this.Id = Id;
            this.D_Name = D_Name;
            this.D_Profile = D_Profile;
            this.D_Description = D_Description;
            this.Number_of_members = Number_of_members;
            this.Number_of_active_members = Number_of_active_members;
            this.Number_of_posts = Number_of_posts;
            this.OwnerId = OwnerId;
            this.IsDeleted = IsDeleted;
            this.Questions = Questions;
            this.Joinings = Joinings;
            this.Created_at = Created_at;
            this.Updated_at = Updated_at;
        }

        public static DiscussionDTO FromDiscussion(Discussion discussion)
        {
            return new DiscussionDTO(
                discussion.Id,
                discussion.D_Name,
                discussion.D_Profile,
                discussion.D_Description,
                discussion.Number_of_members,
                discussion.Number_of_active_members,
                discussion.Number_of_posts,
                discussion.OwnerId,
                discussion.IsDeleted,
                discussion.Questions.Select(q => QuestionDTO.FromQuestion(q)).ToList(),
                discussion.Joinings.Select(j => new JoiningDTO
                {
                    Id = j.Id,
                    JoinedAt = j.JoinedAt,
                    User = new UserMiniDTO
                    {
                        Id = j.User.Id,
                        Email = j.User.Email!,
                        UserName = j.User.UserName!,
                        ProfilePicture = j.User.ProfilePicture,
                    }
                }).ToList(),
                discussion.Created_at,
                discussion.Updated_at
            );
        }
    }

}
