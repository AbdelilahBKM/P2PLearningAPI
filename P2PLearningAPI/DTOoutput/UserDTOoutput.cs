namespace P2PLearningAPI.DTOoutput
{
    public class UserDTOoutput
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string? ProfilePicture { get; set; }
        public string? Bio { get; set; }
        public bool IsActive { get; set; }
        public int numberOfRequests { get; set; }
        public DateTime Last_Login { get; set; }
        public DateTime Created_at { get; set; } = DateTime.Now;
        public DateTime Updated_at { get; set; } = DateTime.Now;

        public UserDTOoutput() { }
        public UserDTOoutput(string id, string firstName, string lastName, string userName, string email, string? profilePicture, string? bio, bool isActive, int numberOfRequests, DateTime last_Login)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            UserName = userName;
            Email = email;
            ProfilePicture = profilePicture;
            Bio = bio;
            IsActive = isActive;
            this.numberOfRequests = numberOfRequests;
            Last_Login = last_Login;
        }
    }
}
