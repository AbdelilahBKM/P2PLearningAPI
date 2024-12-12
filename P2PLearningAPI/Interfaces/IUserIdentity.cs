using P2PLearningAPI.DTOs;
using P2PLearningAPI.Models;

namespace P2PLearningAPI.Interfaces
{
    public interface IUserIdentity
    {
        public ICollection<User> GetUsers();
        public User? GetUser(string id);
        public bool CheckUserExistByEmail(string email);
        public bool CheckUserExistById(string id);
        public (User user, string token) Register(RegisterDTO user);
        public (User user, string token) Login(LoginDTO user);
        public User UpdateUser(string id, UpdateDTO user);
        public string GenerateToken(User user);

    }
}
