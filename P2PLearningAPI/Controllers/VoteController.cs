using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using P2PLearningAPI.DTOs;
using P2PLearningAPI.Interfaces;
using P2PLearningAPI.Models;
using P2PLearningAPI.Repository;
using P2PLearningAPI.Services;

namespace P2PLearningAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoteController : ControllerBase
    {
        private readonly IVoteInterface _voteRepository;
        private readonly INotificationService _notificationService; 
        private readonly PostRepository _postRepository;

        public VoteController(IVoteInterface voteRepository, INotificationService notificationService,
                              
            PostRepository postRepository)
        {
            _voteRepository = voteRepository;
            _notificationService = notificationService;
            _postRepository = postRepository;
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
        public IActionResult CreateVote([FromBody] VoteDTO vote)
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
        // POST: api/Vote/MarkBestAnswer/{postId}
        [Authorize]
        [HttpPost("MarkBestAnswer/{postId}")]
        [ProducesResponseType(200, Type = typeof(Vote))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult MarkBestAnswer(long postId)
        {
            // Retrieve the post by postId (it could be a Question or an Answer)
            var post = _postRepository.GetPost(postId);
            if (post == null)
                return NotFound("Post not found.");

            // Ensure the post is an answer, not a question
            if (!(post is Answer answer))
                return BadRequest("The post is not an answer.");

            // Mark the answer as the best (update the IsBestAnswer property)
            answer.IsBestAnswer = true;

            // Save changes
            if (!_postRepository.Save())
                return StatusCode(500, "Failed to save changes.");

            // Notify the owner of the answer (this could be the user who posted the answer)
            var notificationMessage = $"Your answer to the question \"{answer.Question.Title}\" was marked as the best answer.";
            _notificationService.CreateNotificationAsync(answer.PostedBy.Id, notificationMessage, NotificationType.BestAnswer);

            return Ok("Best answer marked and user notified.");
        }
    }
}
