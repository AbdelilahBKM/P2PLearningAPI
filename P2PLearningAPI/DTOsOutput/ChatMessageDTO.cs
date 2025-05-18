using P2PLearningAPI.Models;

namespace P2PLearningAPI.DTOsOutput
{
    public class ChatMessageDTO
    {
        public long Id { get; set; }
        public ChatRole Role { get; set; }
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }
        public long? SessionId { get; set; }

        public ChatMessageDTO() { 
            Id= 0;
            Role = ChatRole.User;
            Content = string.Empty;
            Timestamp = DateTime.UtcNow;
            SessionId = null;
        }
        public ChatMessageDTO(long id, ChatRole role, string content, DateTime timestamp, long? sessionId)
        {
            Id = id;
            Role = role;
            Content = content;
            Timestamp = timestamp;
            SessionId = sessionId;
        }
        public static ChatMessageDTO FromChatMessage(ChatMessage chatMessage)
        {
            return new ChatMessageDTO
            {
                Id = chatMessage.Id,
                Role = chatMessage.Role,
                Content = chatMessage.Content,
                Timestamp = chatMessage.Timestamp,
                SessionId = chatMessage.SessionId
            };
        }

    }
}
