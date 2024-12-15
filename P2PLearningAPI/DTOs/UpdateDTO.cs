namespace P2PLearningAPI.DTOs
{
    public class UpdateDTO
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string UserName { get; set; }
        public string? ProfilePicture { get; set; }
        public string? Bio { get; set; }
    }
}
