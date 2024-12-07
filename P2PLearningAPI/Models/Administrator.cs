namespace P2PLearningAPI.Models
{
    public class Administrator: User
    {
        public string token { get; set; } = string.Empty;
        public Administrator() { }
        public Administrator(string firstname, string lastname, string email, string? profile_pic = null, string? bio = null) : 
            base(firstname, lastname, email, profile_pic, bio) 
        { 
        }

    }
}
