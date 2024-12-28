using Microsoft.EntityFrameworkCore;
using P2PLearningAPI.Data;
using P2PLearningAPI.Interfaces;
using P2PLearningAPI.Models;

namespace P2PLearningAPI.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly P2PLearningDbContext _context;
        private readonly ITokenService _tokenService;

        public NotificationRepository(P2PLearningDbContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        public async Task AddAsync(Notification notification)
        {
            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Notification>> GetUserNotificationsAsync(string Id, string token)
        {
            var (userId, _) = _tokenService.DecodeToken(token);
            if (userId != Id)
                throw new UnauthorizedAccessException("Unauthorized access");
            return await _context.Notifications
                .Where(n => n.UserId == Id)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        public async Task<Notification> GetByIdAsync(long id, string token)
        {
            Notification notification = await _context.Notifications.FindAsync(id);
            if (notification == null)
                throw new KeyNotFoundException("Notification not found");
            var (userId, _) = _tokenService.DecodeToken(token);
            if (notification.UserId != userId)
                throw new UnauthorizedAccessException("Unauthorized access");
            return notification;
        }

        public async Task UpdateAsync(Notification notification, string token)
        {
            var (userId, _) = _tokenService.DecodeToken(token);
            if (notification.UserId != userId)
                throw new UnauthorizedAccessException("Unauthorized access");
            _context.Entry(notification).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task MarkAllAsRead(string UserId, string token)
        {
            var (userId, _) = _tokenService.DecodeToken(token);
            if (userId != UserId)
                throw new UnauthorizedAccessException("Unauthorized access");
            var notifications = await _context.Notifications
                .Where(n => n.UserId == UserId && !n.IsRead)
                .ToListAsync();
            foreach (var notification in notifications)
            {
                notification.MarkAsRead();
                _context.Entry(notification).State = EntityState.Modified;
            }
            await _context.SaveChangesAsync();
        }
        public async Task DeleteNotification(long id, string token)
        {
            var (userId, _) = _tokenService.DecodeToken(token);
            var notification = await GetByIdAsync(id, token);
            if (notification.UserId != userId)
                throw new UnauthorizedAccessException("Unauthorized access");
            _context.Notifications.Remove(notification);
            await _context.SaveChangesAsync();
        }
    }
}
