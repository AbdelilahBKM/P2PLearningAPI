using P2PLearningAPI.Models;
using P2PLearningAPI.Interfaces;
using P2PLearningAPI.Repository;
using P2PLearningAPI.Repositories;
using System.Diagnostics;
using P2PLearningAPI.DTOs;
using P2PLearningAPI.Data;

namespace P2PLearningAPI.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly P2PLearningDbContext _context;

        public NotificationService(INotificationRepository notificationRepository, P2PLearningDbContext context)
        {
            _notificationRepository = notificationRepository;
            _context = context;
        }

        public async Task CreateNotificationAsync(NotificationDTO notifDTO)
        {
            Notification? notification = null;
            Post? post = null;
            User? user = null;
            Discussion? discussion = null;
            switch (notifDTO.NotificationType)
            {
                case NotificationType.Discussion:
                    discussion = await _context.Discussions.FindAsync(notifDTO.ConcernedDiscussionId);
                    if (discussion != null)
                    {
                        notification = new Notification(
                            notifDTO.UserId,
                            notifDTO.Message,
                            notifDTO.NotificationType,
                            discussion
                        );
                    }
                    break;
                case NotificationType.Reply:
                    post = await _context.Posts.FindAsync(notifDTO.ConcernedPostId);
                    user = await _context.Users.FindAsync(notifDTO.InteractiveUserId);
                    if (post != null && user != null)
                    {
                        notification = new Notification(
                            notifDTO.UserId,
                            notifDTO.Message,
                            notifDTO.NotificationType,
                            post,
                            user
                        );
                        await _notificationRepository.AddAsync(notification);
                    }
                    break;
                case NotificationType.BestAnswer:
                    post = await _context.Posts.FindAsync(notifDTO.ConcernedPostId);
                    if(post != null)
                        notification = new Notification(
                            notifDTO.UserId,
                            notifDTO.Message,
                            notifDTO.NotificationType,
                            post
                        );
                    break;
                default:
                    Debug.WriteLine("Unknown notification type");
                    break;
            }
            if (notification != null)
                await _notificationRepository.AddAsync(notification);
        }

        public async Task CreateNotificationsForUsersAsync(IEnumerable<User> users, string message, NotificationType notificationType, Discussion discussion)
        {
            var notifications = users.Select(user => new Notification(user.Id, message, notificationType, discussion));
            foreach (var notification in notifications)
            {
                await _notificationRepository.AddAsync(notification);
            }
        }
    }
}
