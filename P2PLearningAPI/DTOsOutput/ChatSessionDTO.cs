namespace P2PLearningAPI.DTOsOutput
{
    public class ChatSessionDTO
    {
        public long SessionId { get; set; }
        public string SessionName { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public UserMiniDTO User { get; set; } = new UserMiniDTO();
        public List<ChatMessageDTO> History { get; set; }
        public DateTime CreatedAt { get; set; }

        public ChatSessionDTO()
        {
            History = new List<ChatMessageDTO>();
            CreatedAt = DateTime.Now;
        }
        public ChatSessionDTO(
            long SessionId, 
            string SessionName, 
            string UserId, 
            UserMiniDTO User, 
            List<ChatMessageDTO> History
            )
        {
            this.SessionId = SessionId;
            this.SessionName = SessionName;
            this.UserId = UserId;
            this.User = User;
            this.History = History;
        }

        public static ChatSessionDTO FromChatSession(Models.ChatSession chatSession)
        {
            return new ChatSessionDTO
            {
                SessionId = chatSession.SessionId,
                SessionName = chatSession.SessionName,
                UserId = chatSession.UserId,
                User = new UserMiniDTO(
                    chatSession.User.Id, 
                    chatSession.User.UserName!, 
                    chatSession.User.Email!, 
                    chatSession.User.ProfilePicture
                    ),
                History = chatSession.History.Select(cm => ChatMessageDTO.FromChatMessage(cm)).ToList(),
                CreatedAt = chatSession.CreatedAt
            };
        }

    }
}
