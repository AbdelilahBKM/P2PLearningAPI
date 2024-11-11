namespace P2PLearningAPI.Models
{
    public class Administrator: User
    {
        public string token { get; set; } = string.Empty;
        public Administrator(string firstname, string lastname, string email): base(firstname, lastname, email) 
        { 
        }

    }
}
