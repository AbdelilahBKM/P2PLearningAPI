using P2PLearningAPI.Models;

namespace P2PLearningAPI.Interfaces
{
    public enum UserType
    {
        Scholar,
        Administrator
    }
    public interface UserInterface
    {
        ICollection<User> GetUsers();
        User GetUser(string id);
        bool CheckUserExist(string id);
        bool CheckUserExistByEmail(string email);
        User GetUserByEmail(string email);
        User CreateUser(User user, string password, UserType userType);
        User UpdateUser(User user, UserType userType);
        bool DeleteUser(string id);
        bool save();
    }
}
