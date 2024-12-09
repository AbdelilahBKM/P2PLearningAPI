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
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ICollection<Request>))]
        public IActionResult GetRequests()
        {
            var requests = _requestRepository.GetRequests();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(requests);
        }

        // GET: api/Request/{id}
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Request))]
        [ProducesResponseType(404)]
        public IActionResult GetRequest(long id)
        {
            var request = _requestRepository.GetRequest(id);
            if (request == null)
                return NotFound();

            return Ok(request);
        }

        // GET: api/Request/ByUser/{userId}
        [HttpGet("ByUser/{userId}")]
        [ProducesResponseType(200, Type = typeof(ICollection<Request>))]
        [ProducesResponseType(404)]
        public IActionResult GetRequestsByUser(string userId)
        {
            var requests = _requestRepository.GetRequestsByUser(userId);
            if (requests == null || !ModelState.IsValid)
                return NotFound();

            return Ok(requests);
        }

        // POST: api/Request
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Request))]
        [ProducesResponseType(400)]
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

            var createdRequest = _requestRepository.CreateRequest(
                new Request(
                    request.Topic, 
                    request.Description,
                    request.User
                    )
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

            var updatedRequest = _requestRepository.UpdateRequest(request);
            if (updatedRequest == null)
                return NotFound();

            return Ok(updatedRequest);
        }

        // PUT: api/Request/Approve/{id}
        [HttpPut("Approve/{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult ApproveRequest(long id)
        {
            var success = _requestRepository.ApproveRequest(id);
            if (!success)
                return NotFound();

            return NoContent();
        }

        // PUT: api/Request/Close/{id}
        [HttpPut("Close/{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult CloseRequest(long id)
        {
            var success = _requestRepository.CloseRequest(id);
            if (!success)
                return NotFound();

            return NoContent();
        }

        // DELETE: api/Request/{id}
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteRequest(long id)
        {
            var success = _requestRepository.DeleteRequest(id);
            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}
