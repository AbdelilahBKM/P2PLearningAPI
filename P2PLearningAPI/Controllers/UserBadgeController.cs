using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using P2PLearningAPI.Interfaces;
using P2PLearningAPI.Models;

namespace P2PLearningAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserBadgeController : Controller
    {
        private readonly IUserBadgeInterface _userBadgeRepository;
        public UserBadgeController(IUserBadgeInterface userBadgeRepository)
        {
            _userBadgeRepository = userBadgeRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ICollection<UserBadge>))]
        public IActionResult GetUserBadges()
        {
            var userBadges = _userBadgeRepository.GetUserBadges();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(userBadges);
        }

        [HttpGet("{userId}")]
        [ProducesResponseType(200, Type = typeof(ICollection<UserBadge>))]
        [ProducesResponseType(404)]
        public IActionResult GetUserBadges(string userId)
        {
            var userBadges = _userBadgeRepository.GetUserBadges(userId);
            if (userBadges == null || !ModelState.IsValid)
                return NotFound();
            return Ok(userBadges);
        }

        [HttpGet("{userId}/{badgeId}")]
        [ProducesResponseType(200, Type = typeof(UserBadge))]
        [ProducesResponseType(404)]
        public IActionResult GetUserBadge(string userId, int badgeId)
        {
            var userBadge = _userBadgeRepository.GetUserBadge(userId, badgeId);
            if (userBadge == null || !ModelState.IsValid)
                return NotFound();
            return Ok(userBadge);
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(UserBadge))]
        [ProducesResponseType(400)]
        public IActionResult AddUserBadge([FromBody] UserBadge userBadge)
        {
            var newUserBadge = _userBadgeRepository.AddUserBadge(userBadge);
            if (newUserBadge == null || !ModelState.IsValid)
                return BadRequest();
            return CreatedAtAction(nameof(GetUserBadge), new { 
                userId = newUserBadge.UserId, 
                badgeId = newUserBadge.BadgeId 
            }, newUserBadge);
        }
        [HttpDelete]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult DeleteUserBadge(string userId, int badgeId)
        {
            if (!_userBadgeRepository.DeleteUserBadge(userId, badgeId) || !ModelState.IsValid)
                return BadRequest();
            return Ok();
        }
    }
}
