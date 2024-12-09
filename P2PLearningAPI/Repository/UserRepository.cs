using Microsoft.EntityFrameworkCore;
using P2PLearningAPI.Data;
using P2PLearningAPI.Interfaces;
using P2PLearningAPI.Models;

namespace P2PLearningAPI.Repository
{
    public class UserRepository: UserInterface
    {
        private readonly P2PLearningDbContext context;
        private readonly UserService userService;
        public UserRepository(P2PLearningDbContext context, UserService userService)
        {
            this.context = context;
            this.userService = userService;
        }

        public ICollection<User> GetUsers()
        {
            return context.Users.OrderBy(u => u.Id).ToList();
        }
        public User GetUser(string id)
        {
            return context.Users.Where(u => u.Id == id).FirstOrDefault()!;
        }
        public User GetUserByEmail(string email)
        {
            return context.Users.Where(u => u.Email == email).FirstOrDefault()!;
        }

        public bool CheckUserExist(string id)
        {
            return context.Users.Any(u => u.Id == id);
        }
        public bool CheckUserExistByEmail(string email)
        {
            return context.Users.Any(u => email == u.Email);
        }
        public User CreateUser(User user, string password, UserType userType)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password cannot be null or empty", nameof(password));

            if (CheckUserExist(user.Email!))
                throw new InvalidOperationException("A user with this email already exists.");

            User newUser;

            switch (userType)
            {
                case UserType.Scholar:
                    newUser = new Scholar(user.FirstName, user.LastName, user.Email!, user.profilePicture, user.Bio);
                    break;
                case UserType.Administrator:
                    newUser = new Administrator(user.FirstName, user.LastName, user.Email!, user.profilePicture, user.Bio);
                    break;
                default:
                    throw new ArgumentException("Invalid user type", nameof(userType));
            }

            userService.RegisterUser(newUser, password);
            context.Users.Add(newUser);

            if (save())
                return newUser;

            throw new InvalidOperationException("Failed to save the user to the database.");
        }

        public User UpdateUser(User user, UserType userType)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            if (!CheckUserExist(user.Id))
            {
                throw new InvalidOperationException("User doesn't exist");
            }

            User updateUser = GetUser(user.Id);
            updateUser.Email = user.Email;
            updateUser.FirstName = user.FirstName;
            updateUser.LastName = user.LastName;
            updateUser.profilePicture = user.profilePicture;
            context.Users.Update(updateUser);
            if(save()) return updateUser;
            throw new InvalidOperationException("Failed to update the user to the database.");
            
        }

        public bool DeleteUser(string id)
        {
            User user = GetUser(id);
            if (user == null)
            {
                throw new InvalidOperationException("User doesnt exist");
            }
            context.Users.Remove(user);
            return context.SaveChanges() > 0;

        }

        public bool save()
        {
            return context.SaveChanges() > 0;
        }
    }
}
