using Microsoft.AspNetCore.Mvc;
using P2PLearningAPI.Interfaces;
using P2PLearningAPI.DTOs;
using P2PLearningAPI.Models;
using NuGet.Common;
using Microsoft.AspNetCore.Authorization;

namespace P2PLearningAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserIdentityController: ControllerBase
    {
        private readonly IUserIdentity _userIdentityRepository;
        public UserIdentityController(IUserIdentity userIdentityRepository)
        {
            _userIdentityRepository = userIdentityRepository;
        }
            
        [Authorize]
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ICollection<User>))]
        public IActionResult GetUsers()
        {
            var users = _userIdentityRepository.GetUsers();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(users);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(User))]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        public IActionResult GetUser(string id)
        {
            var user = _userIdentityRepository.GetUser(id);
            if (user == null)
                return NotFound();
            return Ok(user);
        }

        [HttpPost("Login")]
        [ProducesResponseType(200, Type = typeof(User))]
        [ProducesResponseType(404)]
        public IActionResult Login([FromBody] LoginDTO user)
        {
            try
            {
                var (loggedInUser, token) = _userIdentityRepository.Login(user);
                return Ok(new
                    {
                        User = new
                        {
                            loggedInUser.Id,
                            loggedInUser.FirstName,
                            loggedInUser.LastName,
                            loggedInUser.Email,
                            loggedInUser.UserName,
                            loggedInUser.UserType,
                            loggedInUser.ProfilePicture,
                            loggedInUser.Bio
                        },
                        Token = token
                    }
                );
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpPost("Register")]
        [ProducesResponseType(200, Type = typeof(User))]
        [ProducesResponseType(404)]
        public IActionResult Register([FromBody] RegisterDTO user)
        {
            try
            {
                var (registeredUser, token) = _userIdentityRepository.Register(user);
                return Ok(new
                {
                    User = new
                    {
                        registeredUser.Id,
                        registeredUser.FirstName,
                        registeredUser.LastName,
                        registeredUser.Email,
                        registeredUser.UserName,
                        registeredUser.UserType,

                    },
                    Token = token
                });
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }

        [Authorize]
        [HttpPut("Udate")]
        [ProducesResponseType(200, Type = typeof(User))]
        [ProducesResponseType(404)]
        public IActionResult UpdateUser(string id, [FromBody] UpdateDTO user)
        {
            try
            {
                var updatedUser = _userIdentityRepository.UpdateUser(id, user);
                return Ok(updatedUser);
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }
    }
}
