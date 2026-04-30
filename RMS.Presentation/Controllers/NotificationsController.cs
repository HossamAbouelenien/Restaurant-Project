using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMS.ServicesAbstraction.IServices.IHubServices.INotificationServices;
using RMS.Shared.QueryParams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        public NotificationsController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetMyNotifications([FromQuery] NotificationQueryParams queryParams)
        {
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
            await _notificationService.MarkAsReadAsync(id);
            return Ok();
        }
    }
}
