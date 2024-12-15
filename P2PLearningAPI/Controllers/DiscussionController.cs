using Microsoft.AspNetCore.Mvc;
using P2PLearningAPI.Interfaces;
using P2PLearningAPI.Models;
using P2PLearningAPI.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace P2PLearningAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscussionController : ControllerBase
    {
        private readonly IDiscussionInterface _discussionRepository;

        public DiscussionController(IDiscussionInterface discussionRepository)
        {
            _discussionRepository = discussionRepository;
        }

        // GET: api/Discussion
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ICollection<Discussion>))]
        public IActionResult GetDiscussions()
        {
            var discussions = _discussionRepository.GetDiscussions();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(discussions);
        }

        // GET: api/Discussion/{id}
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Discussion))]
        [ProducesResponseType(404)]
        public IActionResult GetDiscussion(long id)
        {
            if (!_discussionRepository.CheckDiscussionExist(id))
                return NotFound();

            var discussion = _discussionRepository.GetDiscussion(id);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(discussion);
        }

        // GET: api/Discussion/ByOwner/{ownerId}
        [HttpGet("ByOwner/{ownerId}")]
        [ProducesResponseType(200, Type = typeof(ICollection<Discussion>))]
        [ProducesResponseType(404)]
        public IActionResult GetDiscussionsByOwner(string ownerId)
        {
            var discussions = _discussionRepository.GetDiscussionsByOwner(ownerId);
            if (discussions == null || !ModelState.IsValid)
                return NotFound();

            return Ok(discussions);
        }

        // POST: api/Discussion
        [Authorize]
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Discussion))]
        [ProducesResponseType(400)]
        public IActionResult CreateDiscussion([FromBody] DiscussionDTO discussionDTO)
        {
            var authHeader = Request.Headers["Authorization"]!;
            string token = authHeader.ToString().Split(" ")[1];
            if (discussionDTO == null)
                return BadRequest("Invalid discussion data.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdDiscussion = _discussionRepository.CreateDiscussion(
                new Discussion(
                    discussionDTO.Owner,
                    discussionDTO.d_Name,
                    discussionDTO.d_Profile
                    ),
                token);
            return CreatedAtAction(nameof(GetDiscussion), new { id = createdDiscussion.Id }, createdDiscussion);
        }

        // PUT: api/Discussion
        [Authorize]
        [HttpPut]
        [ProducesResponseType(200, Type = typeof(Discussion))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateDiscussion([FromBody] Discussion discussion)
        {
            if (discussion == null)
                return BadRequest("Invalid discussion data.");

            if (!_discussionRepository.CheckDiscussionExist(discussion.Id))
                return NotFound();
            var authHeader = Request.Headers["Authorization"]!;
            string token = authHeader.ToString().Split(" ")[1];
            var updatedDiscussion = _discussionRepository.UpdateDiscussion(discussion, token);
            return Ok(updatedDiscussion);
        }

        // DELETE: api/Discussion/{id}
        [Authorize]
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteDiscussion(long id)
        {
            if (!_discussionRepository.CheckDiscussionExist(id))
                return NotFound();
            var authHeader = Request.Headers["Authorization"]!;
            string token = authHeader.ToString().Split(" ")[1];
            if (!_discussionRepository.DeleteDiscussion(id, token))
                return BadRequest("Failed to delete discussion.");

            return NoContent();
        }

        // PATCH: api/Discussion/MarkDeleted/{id}
        [HttpPatch("MarkDeleted/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult MarkDiscussionAsDeleted(long id)
        {
            if (!_discussionRepository.CheckDiscussionExist(id))
                return NotFound();

            if (!_discussionRepository.MarkDiscussionAsDeleted(id))
                return BadRequest("Failed to mark discussion as deleted.");

            return Ok($"Discussion {id} marked as deleted.");
        }

        // GET: api/Discussion/{discussionId}/Questions
        [HttpGet("{discussionId}/Questions")]
        [ProducesResponseType(200, Type = typeof(ICollection<Question>))]
        [ProducesResponseType(404)]
        public IActionResult GetQuestionsByDiscussion(long discussionId)
        {
            var questions = _discussionRepository.GetQuestionsByDiscussion(discussionId);
            if (questions == null || !ModelState.IsValid)
                return NotFound();

            return Ok(questions);
        }
    }
}
