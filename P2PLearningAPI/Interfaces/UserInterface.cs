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
        User GetUser(long id);
        bool CheckUserExist(long id);
        bool CheckUserExist(string email);
        User GetUser(string email);
        User CreateUser(User user, string password, UserType userType);
        User UpdateUser(User user, UserType userType);
        bool DeleteUser(long id);
        bool save();
    }
}
