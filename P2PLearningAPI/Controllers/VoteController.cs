using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using P2PLearningAPI.DTOs;
using P2PLearningAPI.Interfaces;
using P2PLearningAPI.Models;

namespace P2PLearningAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoteController : ControllerBase
    {
        private readonly IVoteInterface _voteRepository;

        public VoteController(IVoteInterface voteRepository)
        {
            _voteRepository = voteRepository;
        }

        // GET: api/Vote
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ICollection<Vote>))]
        public IActionResult GetVotes()
        {
            var votes = _voteRepository.GetVotes();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(votes);
        }

        // GET: api/Vote/{id}
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Vote))]
        [ProducesResponseType(404)]
        public IActionResult GetVote(long id)
        {
            var vote = _voteRepository.GetVote(id);
            if (vote == null)
                return NotFound();

            return Ok(vote);
        }

        // GET: api/Vote/ByPost/{postId}
        [HttpGet("ByPost/{postId}")]
        [ProducesResponseType(200, Type = typeof(ICollection<Vote>))]
        [ProducesResponseType(404)]
        public IActionResult GetVotesByPost(long postId)
        {
            var votes = _voteRepository.GetVotesByPost(postId);
            if (votes == null || !votes.Any())
                return NotFound();

            return Ok(votes);
        }

        // GET: api/Vote/{postId}/{userId}
        [HttpGet("{postId}/{userId}")]
        [ProducesResponseType(200, Type = typeof(Vote))]
        [ProducesResponseType(404)]
        public IActionResult GetVote(long postId, string userId)
        {
            var vote = _voteRepository.GetVote(postId, userId);
            if (vote == null)
                return NotFound();

            return Ok(vote);
        }

        // GET: api/Vote/ByUser/{userId}
        [HttpGet("ByUser/{userId}")]
        [ProducesResponseType(200, Type = typeof(ICollection<Vote>))]
        [ProducesResponseType(404)]
        public IActionResult GetVotesByUser(string userId)
        {
            var votes = _voteRepository.GetVotesByUser(userId);
            if (votes == null || !votes.Any())
                return NotFound();

            return Ok(votes);
        }

        // POST: api/Vote
        [Authorize]
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Vote))]
        [ProducesResponseType(400)]
        public IActionResult CreateVote([FromBody] VoteCreateDTO vote)
        {
            if (vote == null)
                return BadRequest("Invalid vote data.");
            try
            {
                var createdVote = _voteRepository.CreateVote(new Vote(vote.UserId, vote.PostId, vote.VoteType));
                if (createdVote == null)
                    return BadRequest("Unable to create the vote.");

                return CreatedAtAction(nameof(GetVote), new { id = createdVote.Id }, createdVote);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/Vote/{id}
        [Authorize]
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteVote(long id)
        {
            var success = _voteRepository.DeleteVote(id);
            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}
