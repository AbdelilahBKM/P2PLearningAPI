namespace P2PLearningAPI.Models
{
    public class ChatSession
    {
        public long SessionId { get; set; }
        public string SessionName { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public List<ChatMessage> History { get; set; } = new List<ChatMessage>();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ChatSession()
        {
            History = new List<ChatMessage>();
        }

        public ChatSession(long sessionId,string sessionName, User user)
        {
            SessionId = sessionId;
            SessionName = sessionName;
            User = user;
            UserId = user.Id;
            History = new List<ChatMessage>();
        }

        public void AddMessage(ChatMessage message)
        {
            History.Add(message);
        }
        public void ClearHistory()
        {
            History.Clear();
        }

        public long GetSessionId()
        {
            return SessionId;
        }
    }
}
