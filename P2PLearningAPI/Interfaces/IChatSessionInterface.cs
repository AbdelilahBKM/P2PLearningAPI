using P2PLearningAPI.DTOsInput;
using P2PLearningAPI.DTOsOutput;
using P2PLearningAPI.Models;

namespace P2PLearningAPI.Interfaces
{
    public interface IChatSessionInterface
    {
        ICollection<ChatSessionDTO> GetChatSessions();
        ChatSessionDTO? GetChatSession(long id);
        ChatSession? GetFullChatSessionById(long id);
        ChatSessionDTO? GetChatSession(string name);
        bool CheckChatSessionExist(long id);
        ICollection<ChatSessionDTO> GetChatSessionsByOwner(string ownerId, string token);
        ChatSessionDTO CreateChatSession(ChatSessionCreateDTO sessionName, string token);
        ChatSessionDTO UpdateChatSession(string updateSessionName, string token);
        bool ClearChatSessionHistory(long id, string token);
        bool DeleteChatSession(long id, string token);
        ICollection<ChatMessageDTO> GetMessagesByChatSession(long chatSessionId);
        bool Save();
    }
}
