using P2PLearningAPI.Models;
using P2PLearningAPI.Services;
namespace P2PLearningAPI.Repositories
{
    public interface INotificationRepository
    {
        Task AddAsync(Notification notification);
        Task<IEnumerable<Notification>> GetUserNotificationsAsync(string userId, string token);
        Task<Notification> GetByIdAsync(long id, string token);
        Task UpdateAsync(Notification notification, string token);
        Task MarkAllAsRead(string UserId, string token);
        Task DeleteNotification(long id, string token);
    }
}
