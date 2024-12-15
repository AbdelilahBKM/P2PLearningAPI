using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using P2PLearningAPI.DTOs;
using P2PLearningAPI.Interfaces;
using P2PLearningAPI.Models;

namespace P2PLearningAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestController : ControllerBase
    {
        private readonly IRequestInterface _requestRepository;

        public RequestController(IRequestInterface requestRepository)
        {
            _requestRepository = requestRepository;
        }

        // GET: api/Request
        [Authorize]
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ICollection<Request>))]
        [ProducesResponseType(401)]
        public IActionResult GetRequests()
        {
            try
            {
                var authHeader = Request.Headers["Authorization"];
                string token = authHeader.ToString().Split(" ")[1];
                var requests = _requestRepository.GetRequests(token);
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                return Ok(requests);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/Request/{id}
        [Authorize]
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Request))]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        public IActionResult GetRequest(long id)
        {
            try
            {
                var authHeader = Request.Headers["Authorization"];
                string token = authHeader.ToString().Split(" ")[1];
                var request = _requestRepository.GetRequest(id, token);
                if (request == null)
                    return NotFound();

                return Ok(request);
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        // GET: api/Request/ByUser/{userId}
        [Authorize]
        [HttpGet("ByUser/{userId}")]
        [ProducesResponseType(200, Type = typeof(ICollection<Request>))]
        [ProducesResponseType(404)]
        public IActionResult GetRequestsByUser(string userId)
        {
            var authHeader = Request.Headers["Authorization"];
            string token = authHeader.ToString().Split(" ")[1];
            var requests = _requestRepository.GetRequestsByUser(userId, token);
            if (requests == null || !ModelState.IsValid)
                return NotFound();

            return Ok(requests);
        }

        // POST: api/Request
        [Authorize]
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Request))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IActionResult CreateRequest([FromBody] RequestDTO request)
        {
            if (request == null)
                return BadRequest("Invalid request data.");
            if(request.User == null)
                return BadRequest("Invalid User data.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var authHeader = Request.Headers["Authorization"];
                string token = authHeader.ToString().Split(" ")[1];
                var createdRequest = _requestRepository.CreateRequest(
                new Request(
                    request.Topic, 
                    request.Description,
                    request.User
                    ),
                token
                );
            return CreatedAtAction(nameof(GetRequest), new { id = createdRequest.Id }, createdRequest);
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/Request
        [HttpPut]
        [ProducesResponseType(200, Type = typeof(Request))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateRequest([FromBody] Request request)
        {
            if (request == null)
                return BadRequest("Invalid request data.");
            try
            {
                var authHeader = Request.Headers["Authorization"];
                string token = authHeader.ToString().Split(" ")[1];
                var updatedRequest = _requestRepository.UpdateRequest(request, token);
                if (updatedRequest == null)
                    return NotFound();
                return Ok(updatedRequest);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/Request/Approve/{id}
        [Authorize]
        [HttpPut("Approve/{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        public IActionResult ApproveRequest(long id)
        {
            try
            {
                var authHeader = Request.Headers["Authorization"];
                string token = authHeader.ToString().Split(" ")[1];
                var success = _requestRepository.ApproveRequest(id, token);
            if (!success)
                return NotFound();

            return NoContent();
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        // PUT: api/Request/Close/{id}
        [Authorize]
        [HttpPut("Close/{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        public IActionResult CloseRequest(long id)
        {
            try
            {
                var authHeader = Request.Headers["Authorization"];
                string token = authHeader.ToString().Split(" ")[1];
                var success = _requestRepository.CloseRequest(id, token);
            if (!success)
                return NotFound();

            return NoContent();
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        // DELETE: api/Request/{id}
        [Authorize]
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        public IActionResult DeleteRequest(long id)
        {
            try
            {
                var authHeader = Request.Headers["Authorization"];
                string token = authHeader.ToString().Split(" ")[1];
                var success = _requestRepository.DeleteRequest(id, token);
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
