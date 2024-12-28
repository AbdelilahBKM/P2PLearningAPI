using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
        [HttpGet("{userId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> GetUserNotifications(string userId)
        {
            try
            {
                string AuthHeader = Request.Headers["Authorization"]!;
                if (string.IsNullOrEmpty(AuthHeader))
                {
                   throw new Exception("Authorization header is missing");
                }
                string token = AuthHeader.Split(" ")[1];
                IEnumerable<Notification> notifications = await _notificationRepository.GetUserNotificationsAsync(userId, token);
                return Ok(notifications);
            } catch (Exception e)
            {
                return Unauthorized(e.Message);
            }
        }
        [Authorize]
        [HttpPatch("markAsRead/{id}")]
        public async Task<IActionResult> MarkAsRead(long id)
        {
            try
            {
                string AuthHeader = Request.Headers["Authorization"]!;
                if (string.IsNullOrEmpty(AuthHeader))
                {
                    throw new Exception("Authorization header is missing");
                }
                string token = AuthHeader.Split(" ")[1];
                var notification = await _notificationRepository.GetByIdAsync(id, token);
                if (notification == null)
                {
                    return NotFound();
                }
                notification.MarkAsRead();
                await _notificationRepository.UpdateAsync(notification, token);
                return Ok(notification);
            }
            catch (Exception e)
            {
                return Unauthorized(e.Message);
            }
        }
        [Authorize]
        [HttpPatch("markAllAsRead/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> MarkAllAsRead(string id)
        {
            try
            {
                string authHeader = Request.Headers["Authorization"]!;
                if (string.IsNullOrEmpty(authHeader))
                {
                    throw new Exception("Authorization header is missing");
                }
                string token = authHeader.Split(" ")[1];
                await _notificationRepository.MarkAllAsRead(id, token);
                return Ok();
            }
            catch (Exception e)
            {
                return Unauthorized(e.Message);
            }
        }
        [Authorize]
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> DeleteNotification(long id)
        {
            try
            {
                string authHeader = Request.Headers["Authorization"]!;
                if (string.IsNullOrEmpty(authHeader))
                {
                    throw new Exception("Authorization header is missing");
                }
                string token = authHeader.Split(" ")[1];
                await _notificationRepository.DeleteNotification(id, token);
                return NoContent();
            }
            catch (Exception e)
            {
                return Unauthorized(e.Message);
            }
        }

    }
}
