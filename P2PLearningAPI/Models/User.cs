using Microsoft.AspNetCore.Identity;
using System;

namespace P2PLearningAPI.Models
{
    public class User
    {
        public  long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string profilePicture { get; set; }
        public string Bio {  get; set; }
        public bool IsActive { get; set; }
        public ICollection<Joining> Joinings { get; } = new HashSet<Joining>();
        public ICollection<Post> Posts { get; } = new HashSet<Post>();
        public ICollection<Vote> Votes { get; } = new HashSet<Vote>();
        public DateTime Last_Login { get; set; }
        public bool AccountDeleted { get; set; } = false;
        public DateTime Created_at { get; set; } = DateTime.Now;
        public DateTime Updated_at { get; set; }
    }

    public class UserService
    {
        private readonly PasswordHasher<User> _passwordHasher = new PasswordHasher<User>();
        public void RegisterUser(User user, string password)
        {
            user.PasswordHash = _passwordHasher.HashPassword(user, password);
        }

        public bool VerifyPassword(User user, string attempt)
        {
            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, attempt);
            return result == PasswordVerificationResult.Success;
        }
    }
}
