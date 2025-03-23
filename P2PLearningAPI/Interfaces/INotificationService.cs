using P2PLearningAPI.DTOs;
using P2PLearningAPI.Models;

namespace P2PLearningAPI.Services
{
    public interface INotificationService
    {
        Task CreateNotificationAsync(NotificationDTO n);
        Task CreateNotificationsForUsersAsync(IEnumerable<User> users, string message, NotificationType notificationType, Discussion d);
    }
}
