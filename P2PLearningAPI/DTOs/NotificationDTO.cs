using P2PLearningAPI.Models;

namespace P2PLearningAPI.DTOs
{
    public class NotificationDTO
    {
        public string UserId { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; }
        public long? ConcernedDiscussionId { get; set; }
        public long? ConcernedPostId { get; set; }
        public string? InteractiveUserId { get; set; }
        public NotificationType NotificationType { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
