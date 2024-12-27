using Microsoft.AspNetCore.Mvc;
using P2PLearningAPI.Models;
using P2PLearningAPI.Repositories;

namespace P2PLearningAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationRepository _notificationRepository;

        public NotificationController(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserNotifications(string userId)
        {
            var notifications = await _notificationRepository.GetUserNotificationsAsync(userId);
            return Ok(notifications);
        }

        [HttpPost("markAsRead/{id}")]
        public async Task<IActionResult> MarkAsRead(long id)
        {
            var notification = await _notificationRepository.GetByIdAsync(id);
            if (notification == null)
            {
                return NotFound();
            }

            notification.MarkAsRead();
            await _notificationRepository.UpdateAsync(notification);
            return Ok(notification);
        }
    }
}
