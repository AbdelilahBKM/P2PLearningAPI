using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using P2PLearningAPI.DTOs;
using P2PLearningAPI.Interfaces;
using P2PLearningAPI.Models;

namespace P2PLearningAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostInterface _postRepository;

        public PostController(IPostInterface postRepository)
        {
            _postRepository = postRepository;
        }

        // GET: api/Post
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ICollection<Post>))]
        public IActionResult GetPosts()
        {
            var posts = _postRepository.GetPosts();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(posts);
        }

        // GET: api/Post/{id}
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Post))]
        [ProducesResponseType(404)]
        public IActionResult GetPost(long id)
        {
            var post = _postRepository.GetPost(id);
            if (post == null)
                return NotFound();

            return Ok(post);
        }

        // GET: api/Post/ByUser/{userId}
        [HttpGet("ByUser/{userId}")]
        [ProducesResponseType(200, Type = typeof(ICollection<Post>))]
        [ProducesResponseType(404)]
        public IActionResult GetPostsByUser(string userId)
        {
            var posts = _postRepository.GetPostsByUser(userId);
            if (posts == null || !posts.Any())
                return NotFound();

            return Ok(posts);
        }

        // POST: api/Post
        [Authorize]
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Post))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IActionResult CreatePost([FromBody] PostCreateDTO postDTO)
        {
            if (postDTO == null)
                return BadRequest("Invalid post data.");
            try
            {
                var authHeader = Request.Headers["Authorization"];
                string token = authHeader.ToString().Split(" ")[1];
                var createdPost = _postRepository.CreatePost(postDTO, token);

                return CreatedAtAction(nameof(GetPost), new { id = createdPost.Id }, createdPost);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/Post
        [Authorize]
        [HttpPut]
        [ProducesResponseType(200, Type = typeof(Post))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdatePost([FromBody] Post post)
        {
            if (post == null)
                return BadRequest("Invalid post data.");
            try
            {
                var authHeader = Request.Headers["Authorization"];
                string token = authHeader.ToString().Split(" ")[1];
                var updatedPost = _postRepository.UpdatePost(post, token);
                if (updatedPost == null)
                    return NotFound();

                return Ok(updatedPost);
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        // PUT: api/Post/Close/{id}
        [Authorize]
        [HttpPut("Close/{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        public IActionResult ClosePost(long id)
        {
            try
            {
                var authHeader = Request.Headers["Authorization"];
                string token = authHeader.ToString().Split(" ")[1];
                var success = _postRepository.ClosePost(id, token);
                if (!success)
                    return NotFound();

                return NoContent();
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        // PUT: api/Post/Reopen/{id}
        [Authorize]
        [HttpPut("Reopen/{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        public IActionResult ReopenPost(long id)
        {
            try
            {
                var authHeader = Request.Headers["Authorization"];
                string token = authHeader.ToString().Split(" ")[1];
                var success = _postRepository.ReopenPost(id, token);
                if (!success)
                    return NotFound();

                return NoContent();
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        // PUT: api/Post/Vote/{postId}/
        [Authorize]
        [HttpPut("Vote/{postId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult VoteOnPost(long postId, [FromBody] Vote vote)
        {

            var success = _postRepository.VoteOnPost(postId, vote);
            if (!success)
                return NotFound();

            return NoContent();
        }
        // PUT: api/Post/Vote/{postId}/
        [Authorize]
        [HttpPut("Vote/remove/{postId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult DeleteVote(long postId, [FromBody] Vote vote)
        {

            var success = _postRepository.DeleteVote(postId, vote);
            if (!success)
                return NotFound();

            return NoContent();
        }

        //POST: api/Post/MarkAsBestAnswer/{id}
        [Authorize]
        [HttpPost("MarkAsBestAnswer/{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        public IActionResult MarkAsBestAnswer(long id)
        {
            try
            {
                var authHeader = Request.Headers["Authorization"];
                string token = authHeader.ToString().Split(" ")[1];
                var success = _postRepository.MarkAsBestAnswer(id, token);
                if (!success)
                    return NotFound();

                return NoContent();
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        // DELETE: api/Post/{id}
        [Authorize]
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        public IActionResult DeletePost(long id)
        {
            try
            {
                var authHeader = Request.Headers["Authorization"];
                string token = authHeader.ToString().Split(" ")[1];
                var success = _postRepository.DeletePost(id, token);
                if (!success)
                    return NotFound();

                return NoContent();
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }
        }
    }
}
