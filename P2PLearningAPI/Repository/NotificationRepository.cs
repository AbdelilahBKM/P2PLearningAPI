using Microsoft.EntityFrameworkCore;
using P2PLearningAPI.Data;
using P2PLearningAPI.Models;

namespace P2PLearningAPI.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly P2PLearningDbContext _context;

        public NotificationRepository(P2PLearningDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Notification notification)
        {
            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Notification>> GetUserNotificationsAsync(string userId)
        {
            return await _context.Notifications
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        public async Task<Notification> GetByIdAsync(long id)
        {
            return await _context.Notifications.FindAsync(id);
        }

        public async Task UpdateAsync(Notification notification)
        {
            _context.Entry(notification).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
