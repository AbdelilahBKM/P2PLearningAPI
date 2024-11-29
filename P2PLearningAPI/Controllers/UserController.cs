using Microsoft.AspNetCore.Mvc;
using P2PLearningAPI.Data;
using P2PLearningAPI.Interfaces;
using P2PLearningAPI.Models;
using P2PLearningAPI.Repository;

namespace P2PLearningAPI.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly UserInterface userRepository;
        public UserController(UserInterface userRepository)
        {
            this.userRepository = userRepository;
        }
        //GET Request
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ICollection<User>))]
        public IActionResult GetUsers()
        {
            var users = userRepository.GetUsers();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(users);
        }
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(User))]
        public IActionResult GetUser([FromQuery] int id)
        {
            var user = userRepository.GetUser(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        //POST Request
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(User))] // User Exist
        [ProducesResponseType(400)] // User doesn't exist
        public IActionResult CreateUser(
            [FromBody] User user,
            [FromBody] string password,
            [FromQuery] UserType userType)
        {
            if (user == null || string.IsNullOrWhiteSpace(password))
            {
                return BadRequest("User data or password is invalid.");
            }
            try
            {
                var newUser = userRepository.CreateUser(user, password, userType);
                return CreatedAtAction(nameof(GetUser), new { id = newUser.Id }, newUser);
            } catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }

        }

        //PUT Request
        [HttpPut]
        [ProducesResponseType(201, Type = typeof(User))]
        public IActionResult UpdateUser([FromBody] User user, [FromQuery] UserType userType) {
            if (user == null)
            {
                return BadRequest("User data is invalid");
            }
            try
            {
                var updatedUser = userRepository.UpdateUser(user, userType);
                return Ok(updatedUser);
            } catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
        }

        //DELETE Request
        [HttpDelete]
        [ProducesResponseType(204)] // sucess
        [ProducesResponseType(404)] // Not Found
        public IActionResult DeleteUser([FromQuery] long Id)
        {
            try
            {
                var success = userRepository.DeleteUser(Id);
                if (success)
                {
                    return NoContent();
                }
                return NotFound();
            }catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}