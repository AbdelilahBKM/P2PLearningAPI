using P2PLearningAPI.Models;

namespace P2PLearningAPI.DTOs
{
    public class RegisterDTO
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string UserName { get; set; }
        public required string Password { get; set; }
        public required UserType UserType { get; set; }
    }
}
