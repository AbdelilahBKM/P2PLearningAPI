using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using P2PLearningAPI.Interfaces;
using P2PLearningAPI.Models;

namespace P2PLearningAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BadgeController : Controller
    {
        private readonly IBadgeInterface _badgeRepository;
        public BadgeController(IBadgeInterface badgeRepository)
        {
            _badgeRepository = badgeRepository;
        }

        // Get: api/Badge
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ICollection<Badge>))]
        public IActionResult GetBadges()
        {
            var badges = _badgeRepository.GetBadges();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(badges);
        }

        // Get: api/Badge/{id}
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Badge))]
        [ProducesResponseType(404)]
        public IActionResult GetBadge(long id)
        {
            var badge = _badgeRepository.GetBadge(id);
            if (badge == null || !ModelState.IsValid)
                return NotFound();
            return Ok(badge);
        }

        // Post: api/Badge
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Badge))]
        [ProducesResponseType(400)]
        public IActionResult CreateBadge([FromBody] Badge badge, [FromHeader] string token)
        {
            var newBadge = _badgeRepository.CreateBadge(badge, token);
            if (newBadge == null || !ModelState.IsValid)
                return BadRequest();
            return CreatedAtAction(nameof(GetBadge), new { id = newBadge.Id }, newBadge);
        }

        // Put: api/Badge
        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult UpdateBadge([FromBody] Badge badge, [FromHeader] string token)
        {
            if (!_badgeRepository.UpdateBadge(badge, token) || !ModelState.IsValid)
                return BadRequest();
            return Ok();
        }

        // Delete: api/Badge/{id}
        [HttpDelete]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult DeleteBadge(long id, [FromHeader] string token)
        {
            if (!_badgeRepository.DeleteBadge(id, token) || !ModelState.IsValid)
                return BadRequest();
            return Ok();
        }


    }
}
