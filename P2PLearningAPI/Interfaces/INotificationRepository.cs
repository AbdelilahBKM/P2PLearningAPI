using P2PLearningAPI.Models;
using P2PLearningAPI.Services;
namespace P2PLearningAPI.Repositories
{
    public interface INotificationRepository
    {
        Task AddAsync(Notification notification);
        Task<IEnumerable<Notification>> GetUserNotificationsAsync(string userId);
        Task<Notification> GetByIdAsync(long id);
        Task UpdateAsync(Notification notification);
    }
}
