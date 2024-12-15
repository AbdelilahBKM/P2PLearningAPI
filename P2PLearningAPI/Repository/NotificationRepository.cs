using P2PLearningAPI.Data;
using P2PLearningAPI.Interfaces;
using P2PLearningAPI.Models;
using P2PLearningAPI.DTOs;

namespace P2PLearningAPI.Repository
{
    public class NotificationRepository : INotificationService
    {
        private readonly P2PLearningDbContext _context;
        private readonly ITokenService _tokenService;

        public NotificationRepository(P2PLearningDbContext context, ITokenService tokenServices, IUserIdentity userIdentity)
        {
            _context = context;
            _tokenService = tokenServices;
            _userIdentity = userIdentity;
        }
        public bool CreateNotification(NotificationDTO notificationDTO)
        {
            var user = _context.Users.Find(notificationDTO.UserId);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {notificationDTO.UserId} not found.");
            }
            _context.Notifications.Add();
            return Save();
        }

        public bool DeleteNotification(long id, string token)
        {
            (var UserId, var _, var _) = _tokenService.DecodeToken(token);
            var notification = _context.Notifications.Find(id);
            if (notification == null)
            {
                return false;
            }
            if (notification.UserId != UserId)
            {
                throw new UnauthorizedAccessException("Unauthorized User");
            }
            _context.Notifications.Remove(notification);
            if(Save())
            {
                return true;
            }
            throw new Exception("Error Deleting Notification");
        }

        public Notification? GetNotification(long id)
        {
            return _context.Notifications.Find(id);
        }

        public ICollection<Notification> GetNotificationsByUserId(string Id, string token)
        {
            (var UserId, var _, var _) = _tokenService.DecodeToken(token);
            if (UserId != Id)
            {
                throw new UnauthorizedAccessException("Unauthorized User");
            }
            var notifications = _context.Users
                .Where(u => u.Id == Id)
                .SelectMany(u => u.Notifications)
                .ToList();
            if (notifications == null)
            {
                throw new Exception("No Notifications Found for the user");
            }
            return notifications;
        }

        public bool MarkNotificationAsRead(long id, string token)
        {
            var (userId, _, _) = _tokenService.DecodeToken(token);
            var notification = _context.Notifications.Find(id);
            if (notification == null)
            {
                throw new KeyNotFoundException($"Notification with ID {id} not found.");
            }
            if (notification.UserId != userId)
            {
                throw new UnauthorizedAccessException("User does not own this notification.");
            }
            notification.MarkAsRead();
            var changesSaved = _context.SaveChanges() > 0;
            if (!changesSaved)
            {
                throw new InvalidOperationException("Failed to mark notification as read.");
            }
            return true;
        }
        public bool MarkAllNotificationsAsRead(string Id, string token)
        {
            (var userId, _, _) = _tokenService.DecodeToken(token);
            if (userId != Id)
            {
                throw new UnauthorizedAccessException("Unauthorized User");
            }
            var notifications = _context.Notifications
                .Where(n => n.UserId == userId)
                .ToList();
            if (notifications == null)
            {
                throw new Exception("No Notifications Found for the user");
            }
            foreach (var notification in notifications)
            {
                notification.MarkAsRead();
            }
            return Save();
        }
        public bool Save()
        {
            return _context.SaveChanges() >= 0;
        }
    }
}
