namespace P2PLearningAPI.Models
{
    public enum ChatRole
    {
        User,
        Assistant,
    }
    public class ChatMessage
    {
        public long Id { get; set; }
        public ChatRole Role { get; set; }
        public string Content { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public long? SessionId { get; set; }
        public ChatSession Session { get; set; } = null!;

        public ChatMessage() { }
        public ChatMessage(ChatRole role, string content, ChatSession session)
        {
            Role = role;
            Content = content;
            Session = session;
            SessionId = session.SessionId;
        }
    }
}
