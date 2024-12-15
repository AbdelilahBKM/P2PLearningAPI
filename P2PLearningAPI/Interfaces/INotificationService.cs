using P2PLearningAPI.Models;
using P2PLearningAPI.DTOs;
namespace P2PLearningAPI.Interfaces
{
    public interface INotificationService
    {
        public ICollection<Notification> GetNotificationsByUserId(string Id, string token);
        public Notification? GetNotification(long id);
        public bool CreateNotification(NotificationDTO notificationDTO);
        public bool MarkNotificationAsRead(long id, string token);
        public bool MarkAllNotificationsAsRead(string userId,string token);
        public bool DeleteNotification(long id, string token);
        public bool Save();
    }
}
