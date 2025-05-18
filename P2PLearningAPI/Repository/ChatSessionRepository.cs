using Microsoft.EntityFrameworkCore;
using NuGet.Common;
using P2PLearningAPI.Data;
using P2PLearningAPI.DTOsInput;
using P2PLearningAPI.DTOsOutput;
using P2PLearningAPI.Interfaces;
using P2PLearningAPI.Models;

namespace P2PLearningAPI.Repository
{
    public class ChatSessionRepository : IChatSessionInterface
    {
        private readonly P2PLearningDbContext _context;
        private readonly ITokenService _tokenService;

        public ChatSessionRepository(P2PLearningDbContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        public bool CheckChatSessionExist(long id)
        {
            return _context.ChatSessions.Any(cs => cs.SessionId == id);
        }

        public ChatSessionDTO CreateChatSession(ChatSessionCreateDTO sessionDTO, string token)
        {
            (var UserId, var _) = _tokenService.DecodeToken(token);
            if (sessionDTO == null)
                throw new ArgumentNullException(nameof(sessionDTO));
            if (string.IsNullOrEmpty(sessionDTO.SessionName))
                throw new ArgumentNullException(nameof(sessionDTO.SessionName));
            User? user = _context.Users.FirstOrDefault(u => u.Id == UserId);
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            ChatSession newSession = new ChatSession(0, sessionDTO.SessionName, user);
            _context.ChatSessions.Add(newSession);
            if (Save())
                return ChatSessionDTO.FromChatSession(newSession);
            throw new Exception("Failed to create chat session.");
        }

        public bool DeleteChatSession(long id, string token)
        {
            (var UserId, var _) = _tokenService.DecodeToken(token);
            ChatSession? chatSession = _context.ChatSessions.FirstOrDefault(cs => cs.SessionId == id && cs.UserId == UserId);
            if (chatSession == null)
                throw new ArgumentNullException(nameof(chatSession));
            if(chatSession.UserId != UserId)
                throw new UnauthorizedAccessException("You do not have permission to delete this chat session.");
            _context.ChatSessions.Remove(chatSession);
            return Save();
        }

        public ChatSessionDTO? GetChatSession(long id)
        {
            ChatSession? chatSession = _context.ChatSessions
                .Include(cs => cs.User)
                .Include(cs => cs.History)
                .FirstOrDefault(cs => cs.SessionId == id);
            if (chatSession == null)
                return null;
            return ChatSessionDTO.FromChatSession(chatSession);
        }

        public ChatSessionDTO? GetChatSession(string name)
        {
            ChatSession? chatSession = _context.ChatSessions
                .Include(cs => cs.User)
                .Include(cs => cs.History)
                .FirstOrDefault(cs => cs.SessionName == name);
            if (chatSession == null)
                return null;
            return ChatSessionDTO.FromChatSession(chatSession);
        }

        public ICollection<ChatSessionDTO> GetChatSessions()
        {
            return _context.ChatSessions
                .Include(cs => cs.User)
                .Include(cs => cs.History)
                .Select(cs => ChatSessionDTO.FromChatSession(cs))
                .ToList();
        }

        public ICollection<ChatSessionDTO> GetChatSessionsByOwner(string ownerId, string token)
        {
            (var UserId, var _) = _tokenService.DecodeToken(token);
            if (ownerId != UserId)
                throw new UnauthorizedAccessException("You do not have permission to access this user's chat sessions.");
            return _context.ChatSessions
                .Include(cs => cs.User)
                .Include(cs => cs.History)
                .ToList()
                .Where(cs => cs.UserId == ownerId)
                .Select(cs => ChatSessionDTO.FromChatSession(cs))
                .ToList();
        }

        public ChatSession? GetFullChatSessionById(long id)
        {
            return _context.ChatSessions
                .Include(cs => cs.User)
                .Include(cs => cs.History)
                .FirstOrDefault(cs => cs.SessionId == id);
        }

        public ICollection<ChatMessageDTO> GetMessagesByChatSession(long chatSessionId)
        {
            return _context.ChatMessages
                .Where(cm => cm.SessionId == chatSessionId)
                .OrderBy(cm => cm.Timestamp)
                .Select(cm => new ChatMessageDTO
                {
                    Id = cm.Id,
                    Role = cm.Role,
                    Content = cm.Content,
                    Timestamp = cm.Timestamp,
                    SessionId = cm.SessionId
                })
                .ToList();
        }

        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }

        public ChatSessionDTO UpdateChatSession(string sessionName, string token)
        {
            (var UserId, var _) = _tokenService.DecodeToken(token);
            ChatSession? chatSession = _context.ChatSessions
                .FirstOrDefault(cs => cs.SessionName == sessionName && cs.UserId == UserId);
            if (chatSession == null)
                throw new ArgumentNullException(nameof(chatSession));
            if (chatSession.UserId != UserId)
                throw new UnauthorizedAccessException("You do not have permission to update this chat session.");
            chatSession.SessionName = sessionName;
            _context.ChatSessions.Update(chatSession);
            if (Save())
                return ChatSessionDTO.FromChatSession(chatSession);
            throw new Exception("Failed to update chat session.");
        }
        public bool ClearChatSessionHistory(long id, string token)
        {
            (var UserId, var _) = _tokenService.DecodeToken(token);
            ChatSession? chatSession = _context.ChatSessions
                .Include(cs => cs.History)
                .FirstOrDefault(cs => cs.SessionId == id && cs.UserId == UserId);
            if (chatSession == null)
                throw new ArgumentNullException(nameof(chatSession));
            if (chatSession.UserId != UserId)
                throw new UnauthorizedAccessException("You do not have permission to clear this chat session's history.");
            foreach (var message in chatSession.History)
            {
                _context.ChatMessages.Remove(message);
            }
            chatSession.ClearHistory();
            _context.ChatSessions.Update(chatSession);
            return Save();
        }
    }
}
