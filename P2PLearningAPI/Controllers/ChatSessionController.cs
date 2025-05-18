using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using P2PLearningAPI.DTOsInput;
using P2PLearningAPI.DTOsOutput;
using P2PLearningAPI.Interfaces;

namespace P2PLearningAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatSessionController : Controller
    {
        private readonly IChatSessionInterface _chatSessionRepository;
        private readonly ITokenService _tokenService;
        public ChatSessionController(IChatSessionInterface chatSessionRepository, ITokenService tokenService)
        {
            _chatSessionRepository = chatSessionRepository;
            _tokenService = tokenService;
        }
        // GET: api/ChatSession
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ICollection<ChatSessionDTO>))]
        public IActionResult GetChatSessions()
        {
            var chatSessions = _chatSessionRepository.GetChatSessions();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(chatSessions);
        }
        // GET: api/ChatSession/{id}
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(ChatSessionDTO))]
        [ProducesResponseType(404)]
        public IActionResult GetChatSession(long id)
        {
            if (!_chatSessionRepository.CheckChatSessionExist(id))
                return NotFound();
            var chatSession = _chatSessionRepository.GetChatSession(id);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(chatSession);
        }
        // GET: api/ChatSession/Name/{name}
        [HttpGet("Name/{name}")]
        [ProducesResponseType(200, Type = typeof(ChatSessionDTO))]
        [ProducesResponseType(404)]
        public IActionResult GetChatSession(string name)
        {
            var decodedName = Uri.UnescapeDataString(name);
            var chatSession = _chatSessionRepository.GetChatSession(decodedName);
            if (chatSession == null || !ModelState.IsValid)
                return NotFound();
            return Ok(chatSession);
        }

        // GET: api/ChatSession/User/{userId}
        [Authorize]
        [HttpGet("User/{userId}")]
        [ProducesResponseType(200, Type = typeof(ICollection<ChatSessionDTO>))]
        [ProducesResponseType(404)]
        public IActionResult GetChatSessionsByOwner(string userId)
        {
            var authHeader = Request.Headers["Authorization"].FirstOrDefault();
            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
            {
                return BadRequest("Authorization header is missing or invalid.");
            }
            string token = authHeader.ToString().Split(" ")[1];
            var chatSessions = _chatSessionRepository.GetChatSessionsByOwner(userId, token);
            if (chatSessions == null || !ModelState.IsValid)
                return NotFound();
            return Ok(chatSessions);
        }
        // POST: api/ChatSession
        [Authorize]
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(ChatSessionDTO))]
        [ProducesResponseType(400)]
        public IActionResult CreateChatSession([FromBody] ChatSessionCreateDTO chatSessionDTO)
        {
            var authHeader = Request.Headers["Authorization"].FirstOrDefault();
            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
            {
                return BadRequest("Authorization header is missing or invalid.");
            }
            string token = authHeader.ToString().Split(" ")[1];
            var chatSession = _chatSessionRepository.CreateChatSession(chatSessionDTO, token);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return CreatedAtAction(nameof(GetChatSession), new { id = chatSession.SessionId }, chatSession);
        }

        // PUT: api/ChatSession/{id}
        [Authorize]
        [HttpPut("{id}")]
        [ProducesResponseType(200, Type = typeof(ChatSessionDTO))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateChatSession(long id, [FromBody] ChatSessionCreateDTO chatSessionDTO)
        {
            var authHeader = Request.Headers["Authorization"].FirstOrDefault();
            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
            {
                return BadRequest("Authorization header is missing or invalid.");
            }
            string token = authHeader.ToString().Split(" ")[1];
            var chatSession = _chatSessionRepository.UpdateChatSession(chatSessionDTO.SessionName, token);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(chatSession);
        }
        // DELETE: api/ChatSession/{id}
        [Authorize]
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteChatSession(long id)
        {
            var authHeader = Request.Headers["Authorization"].FirstOrDefault();
            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
            {
                return BadRequest("Authorization header is missing or invalid.");
            }
            string token = authHeader.ToString().Split(" ")[1];
            if (!_chatSessionRepository.CheckChatSessionExist(id))
                return NotFound();
            var chatSession = _chatSessionRepository.DeleteChatSession(id, token);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return NoContent();

        }

        // GET: api/ChatSession/History/{id}
        [HttpGet("History/{id}")]
        [ProducesResponseType(200, Type = typeof(ICollection<ChatMessageDTO>))]
        [ProducesResponseType(404)]
        public IActionResult GetMessagesByChatSession(long id)
        {
            if (!_chatSessionRepository.CheckChatSessionExist(id))
                return NotFound();
            var chatMessages = _chatSessionRepository.GetMessagesByChatSession(id);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(chatMessages);
        }

        // PUT: api/ChatSession/ClearHistory/{id}
        [Authorize]
        [HttpPut("ClearHistory/{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult ClearChatSessionHistory(long id)
        {
            var authHeader = Request.Headers["Authorization"].FirstOrDefault();
            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
            {
                return BadRequest("Authorization header is missing or invalid.");
            }
            string token = authHeader.ToString().Split(" ")[1];
            if (!_chatSessionRepository.CheckChatSessionExist(id))
                return NotFound();
            var chatSession = _chatSessionRepository.ClearChatSessionHistory(id, token);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return NoContent();
        }
    }
}
