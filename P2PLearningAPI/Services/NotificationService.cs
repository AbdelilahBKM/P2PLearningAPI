using P2PLearningAPI.Models;
using P2PLearningAPI.Interfaces;
using P2PLearningAPI.Repository;
using P2PLearningAPI.Repositories;

namespace P2PLearningAPI.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;

        public NotificationService(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task CreateNotificationAsync(string userId, string message, NotificationType notificationType)
        {
            var notification = new Notification(userId, message, notificationType);
            await _notificationRepository.AddAsync(notification);
        }

        public async Task CreateNotificationsForUsersAsync(IEnumerable<User> users, string message, NotificationType notificationType)
        {
            var notifications = users.Select(user => new Notification(user.Id, message, notificationType));
            foreach (var notification in notifications)
            {
                await _notificationRepository.AddAsync(notification);
            }
        }
    }
}
