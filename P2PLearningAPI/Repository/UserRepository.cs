using P2PLearningAPI.Data;
using P2PLearningAPI.Interfaces;
using P2PLearningAPI.Models;

namespace P2PLearningAPI.Repository
{
    public class UserRepository: UserInterface
    {
        private readonly P2PLearningDbContext context;
        public UserRepository(P2PLearningDbContext context){
            this.context = context;
        }

        public ICollection<User> GetUsers()
        {
            return context.Users.OrderBy(u => u.Id).ToList();
        }
        public User GetUser(int id)
        {
            return context.Users.Where(u => u.Id == id).FirstOrDefault();
        }
        public User GetUser(string email)
        {
            return context.Users.Where(u => u.Email == email).FirstOrDefault();
        }

        public bool CheckUserExist(int id)
        {
            return context.Users.Any(u => u.Id == id);
        }
        public bool CheckUserExist(string email)
        {
            return context.Users.Any(u => email == u.Email);
        }
        public User CreateUser(User user, string password, UserType userType)
        {
            try
            {
                if (user == null)
                    throw new ArgumentNullException(nameof(user));
                if (password == null)
                    throw new ArgumentNullException(nameof(password));

                UserService userService = new UserService();
                User newUser;

                switch (userType) {
                    case UserType.Scholar:
                        newUser = new Scholar(user.FirstName, user.LastName, user.Email);
                        break;
                    case UserType.Administrator:
                        newUser = new Administrator(user.FirstName, user.LastName, user.Email);
                        break;
                    default:
                        throw new ArgumentException("invalid userType", nameof(userType));
                }
                context.Users.Add(newUser);
                return newUser;


            }catch (ArgumentNullException ex)
            {
                throw new InvalidOperationException("Invalid input parameters", ex);
            }
            catch (ArgumentException ex)
            {
                throw new InvalidOperationException("Invalid userType", ex);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while creating the user", ex);
            }
        }

        public User UpdateUser(User user, UserType userType)
        {
            throw new NotImplementedException();
        }

        public bool DeleteUser(int id)
        {
            throw new NotImplementedException();
        }

        public bool save()
        {
            throw new NotImplementedException();
        }
    }
}
