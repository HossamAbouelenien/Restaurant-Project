using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RMS.ServicesAbstraction.IServices.IHubServices.INotificationServices;
using RMS.Shared.QueryParams;
using System.Security.Claims;

namespace RMS.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        private readonly ILogger<NotificationsController> _logger;
        public NotificationsController(INotificationService notificationService, ILogger<NotificationsController> logger)
        {
            _notificationService = notificationService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetMyNotifications([FromQuery] NotificationQueryParams queryParams)
        {
            _logger.LogInformation("GetMyNotifications request started");
            var queryParamsWithUserInfo = new NotificationQueryParams
            {
                UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                Role = User.FindFirst(ClaimTypes.Role)?.Value,
                BranchId = queryParams.BranchId
            };
            var result = await _notificationService.GetAllAsync(queryParamsWithUserInfo);
            return Ok(result);
        }


        [HttpPut("{id}/read")]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            _logger.LogInformation("MarkNotificationAsRead request started");
            await _notificationService.MarkAsReadAsync(id);
            return Ok();
        }
    }
}
