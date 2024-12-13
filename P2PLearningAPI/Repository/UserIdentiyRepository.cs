using P2PLearningAPI.DTOs;
using P2PLearningAPI.Interfaces;
using P2PLearningAPI.Models;
using P2PLearningAPI.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace P2PLearningAPI.Repository
{
    public class UserIdentiyRepository : IUserIdentity
    {
        private readonly P2PLearningDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ITokenService _tokenService;
        public UserIdentiyRepository(
            P2PLearningDbContext context,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ITokenService tokenService
            )
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }
        public bool CheckUserExistByEmail(string email)
        {
            return _context.Users.Any(u => u.Email == email);
        }

        public bool CheckUserExistById(string id)
        {
            return _context.Users.Any(u => u.Id == id);
        }

        public User? GetUser(string id, string token)
        {
            var (UserId, _, _) = _tokenService.DecodeToken(token);
            if(UserId != id)
                throw new Exception("Unauthorized Access!");
            return _context.Users.Find(id);
        }

        public ICollection<User> GetUsers()
        {
            return _context.Users.ToList();
        }

        public (User user, string token) Login(LoginDTO userDTO)
        {
            
            var user = _userManager.FindByEmailAsync(userDTO.Email).Result;
            if (user == null)
                throw new Exception("User Not Found!");
            var result = _signInManager.CheckPasswordSignInAsync(user, userDTO.Password, false).Result;
            if (result.Succeeded)
            {
                var token = _tokenService.GenerateToken(user);
                return (user, token);
            }

            throw new Exception("Invalid Password!");
        }

        public (User user, string token) Register(RegisterDTO user)
        {
            var User = new User
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                UserName = user.Email,
                UserType = user.UserType
            };
            var result = _userManager.CreateAsync(User, user.Password).Result;
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Unable to create user: {errors}");
            }
            var token = _tokenService.GenerateToken(User);
            return (User, token);
        }

        public User UpdateUser(string id, UpdateDTO userDTO)
        {
            var user = _context.Users.Find(id);
            if (user == null)
                throw new Exception("User Not Found!");
            user.FirstName = userDTO.FirstName;
            user.LastName = userDTO.LastName;
            user.UserName = userDTO.UserName;
            user.ProfilePicture = userDTO.ProfilePicture;
            user.Bio = userDTO.Bio;
            _context.SaveChanges();
            return user;
        }

        
    }
}
