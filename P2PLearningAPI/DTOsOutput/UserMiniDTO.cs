namespace P2PLearningAPI.DTOsOutput
{
    public class UserMiniDTO
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string? ProfilePicture { get; set; }

        public UserMiniDTO()
        {
            Id = string.Empty;
            UserName = string.Empty;
            Email = string.Empty;
            ProfilePicture = null;
        }

        public UserMiniDTO(string id, string userName, string email, string? profilePicture)
        {
            Id = id;
            this.UserName = userName;
            Email = email;
            ProfilePicture = profilePicture;
        }
    }

}
