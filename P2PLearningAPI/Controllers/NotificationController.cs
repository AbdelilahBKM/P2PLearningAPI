using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using P2PLearningAPI.Interfaces;
using P2PLearningAPI.Models;
namespace P2PLearningAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : Controller
    {
        private readonly INotificationService _notificationService;
        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        // GET: api/Notification
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Notification))]
        [ProducesResponseType(404)]
        public IActionResult GetNotifications(long id)
        {
            var notifications = _notificationService.GetNotification(id);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(notifications);
        }
        //GET: api/Notification/ByUserId/{userId}
        [Authorize]
        [HttpGet("ByUserId/{userId}")]
        [ProducesResponseType(200, Type = typeof(ICollection<Notification>))]
        public IActionResult GetNotificationsByUserId(string userId)
        {
            try
            {
                var authHeader = Request.Headers["Authorization"];
                var token = authHeader.ToString().Substring("Bearer ".Length).Trim();
                var notifications = _notificationService.GetNotificationsByUserId(userId, token);
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                return Ok(notifications);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        // POST: api/Notification
        [Authorize]
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public IActionResult CreateNotification([FromBody] Notification notification)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_notificationService.CreateNotification(notification))
                return BadRequest();

            return CreatedAtAction("GetNotifications", new { id = notification.Id }, notification);
        }

        // PUT: api/Notification/MarkAsRead/{id}
        [Authorize]
        [HttpPut("MarkAsRead/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult MarkNotificationAsRead(long id)
        {
            try
            {
                var authHeader = Request.Headers["Authorization"];
                var token = authHeader.ToString().Substring("Bearer ".Length).Trim();
                if (!_notificationService.MarkNotificationAsRead(id, token))
                    return BadRequest();

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        // PUT: api/Notification/MarkAllAsRead/{userId}
        [Authorize]
        [HttpPut("MarkAllAsRead/{userId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult MarkAllNotificationsAsRead(string userId)
        {
            try
            {
                var authHeader = Request.Headers["Authorization"];
                var token = authHeader.ToString().Substring("Bearer ".Length).Trim();
                if (!_notificationService.MarkAllNotificationsAsRead(userId, token))
                    return BadRequest();

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        // DELETE: api/Notification/{id}
        [Authorize]
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult DeleteNotification(long id)
        {
            try
            {
                var authHeader = Request.Headers["Authorization"];
                var token = authHeader.ToString().Substring("Bearer ".Length).Trim();
                if (!_notificationService.DeleteNotification(id, token))
                    return BadRequest();

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
