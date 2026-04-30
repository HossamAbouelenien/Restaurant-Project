using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RMS.ServicesAbstraction.IServices.IReportServices;
using RMS.Shared.DTOs.Utility;

namespace RMS.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _reportService;
        private readonly ILogger<ReportsController> _logger;
        public ReportsController(IReportService reportService, ILogger<ReportsController> logger)
        {
            _reportService = reportService;
            _logger = logger;
        }

        //[Authorize(Roles = SD.Role_Admin)]
        [HttpGet("dashboard")]
        public async Task <IActionResult> GetReport()
        {
            _logger.LogInformation("GetDashboardReport request started");
            var reportData = await _reportService.GetDashboardAsync();
            return Ok(reportData);
        }


        //[Authorize(Roles = SD.Role_Admin)]
        [HttpGet("revenue")]
        public async Task<IActionResult> GetRevenue(
            [FromQuery] int? branchId,
            [FromQuery] DateTime? from,
            [FromQuery] DateTime? to)
        {
            _logger.LogInformation("GetRevenueReport request started");
            var result = await _reportService.GetRevenueAsync(branchId, from, to);
            return Ok(result);
        }


        //[Authorize(Roles = SD.Role_Admin)]
        [HttpGet("orders-by-type")]
        public async Task<IActionResult> GetOrdersByType()
        {
            _logger.LogInformation("GetOrdersByTypeReport request started");
            var result = await _reportService.GetOrdersByTypeAsync();
            return Ok(result);
        }


        //[Authorize(Roles = SD.Role_Admin)]
        [HttpGet("top-items")]
        public async Task<IActionResult> GetTopItems([FromQuery] int top = 5)
        {
            _logger.LogInformation("GetTopItemsReport request started");
            var result = await _reportService.GetTopItemsAsync(top);
            return Ok(result);
        }


        //[Authorize(Roles = SD.Role_Admin + "" + SD.Role_Chef)]
        [HttpGet("inventory-usage")]
        public async Task<IActionResult> GetInventoryUsage()
        {
            _logger.LogInformation("GetInventoryUsageReport request started");
            var result = await _reportService.GetInventoryUsageAsync();
            return Ok(result);
        }

        //[Authorize(Roles = SD.Role_Admin + "" + SD.Role_Driver)]
        [HttpGet("daily-revenue")]
        public async Task<IActionResult> GetDailyRevenue([FromQuery] int? branchId)
        {
            _logger.LogInformation("GetDailyRevenueReport request started");
            var result = await _reportService.GetDailyRevenueLast7DaysAsync(branchId);
            return Ok(result);
        }



    }
}





















