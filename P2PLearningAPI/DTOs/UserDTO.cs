using P2PLearningAPI.Interfaces;
using P2PLearningAPI.Models;

namespace P2PLearningAPI.DTOs
{
    public class UserDTO
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public string? ProfilePicture { get; set; }
        public string? Bio { get; set; }
        public required string Password { get; set; }
        public required UserType UserType { get; set; }
    }
}
