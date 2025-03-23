namespace P2PLearningAPI.Models
{
    public enum NotificationType { 
        Joining, // a user joins your discussion discussion: 0
        Vote, // a user votes on your post: 1
        Post, // a user posts on your discussion: 2
        Comment, // a user comments on your post or answer: 3
        Reply, // a user replies to your post/answer : 4
        Discussion, // an admin approves your request to create discussion : 5
        BestAnswer, // your answer is marked as the best answer : 6
    }
    public class Notification
    {
        public long Id { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; }
        public Discussion? ConcernedDiscussion { get; set; }
        public Post? ConcernedPost { get; set; }
        public User? InteractiveUser { get; set; }
        public NotificationType NotificationType { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public Notification()
        {
        }
        // constructor for notifications related to discussions
        public Notification(string UserId, string message, NotificationType notificationType, Discussion discussion)
        {
            this.UserId = UserId;
            Message = message;
            NotificationType = notificationType;
            ConcernedDiscussion = discussion;
        }
        // constructor for notifications related to posts reply
        public Notification(string UserId, string message, NotificationType notificationType, Post post, User user)
        {
            this.UserId = UserId;
            Message = message;
            NotificationType = notificationType;
            ConcernedPost = post;
            InteractiveUser = user;
        }

        // constructor for notifications related to post mark as best answer
        public Notification(string UserId, string message, NotificationType notificationType, Post post)
        {
            this.UserId = UserId;
            Message = message;
            NotificationType = notificationType;
            ConcernedPost = post;
        }

        public void MarkAsRead()
        {
            IsRead = true;
            UpdatedAt = DateTime.Now;
        }
    }
}
