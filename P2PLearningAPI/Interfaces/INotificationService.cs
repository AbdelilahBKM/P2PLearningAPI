using P2PLearningAPI.Models;

namespace P2PLearningAPI.Services
{
    public interface INotificationService
    {
        Task CreateNotificationAsync(string userId, string message, NotificationType notificationType);
        Task CreateNotificationsForUsersAsync(IEnumerable<User> users, string message, NotificationType notificationType);
    }
}
