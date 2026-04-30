using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMS.ServicesAbstraction.IServices.IReportServices;
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
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _reportService;
        public ReportsController(IReportService reportService)
        {
            _reportService = reportService;
        }


        [HttpGet("dashboard")]
        public async Task <IActionResult> GetReport()
        {
            var reportData = await _reportService.GetDashboardAsync();
            return Ok(reportData);
        }



        [HttpGet("revenue")]
        public async Task<IActionResult> GetRevenue(
            [FromQuery] int? branchId,
            [FromQuery] DateTime? from,
            [FromQuery] DateTime? to)
        {
            var result = await _reportService.GetRevenueAsync(branchId, from, to);
            return Ok(result);
        }



        [HttpGet("orders-by-type")]
        public async Task<IActionResult> GetOrdersByType()
        {
            var result = await _reportService.GetOrdersByTypeAsync();
            return Ok(result);
        }



        [HttpGet("top-items")]
        public async Task<IActionResult> GetTopItems([FromQuery] int top = 5)
        {
            var result = await _reportService.GetTopItemsAsync(top);
            return Ok(result);
        }



        [HttpGet("inventory-usage")]
        public async Task<IActionResult> GetInventoryUsage()
        {
            var result = await _reportService.GetInventoryUsageAsync();
            return Ok(result);
        }

        [HttpGet("daily-revenue")]
        public async Task<IActionResult> GetDailyRevenue([FromQuery] int? branchId)
        {
            var result = await _reportService.GetDailyRevenueLast7DaysAsync(branchId);
            return Ok(result);
        }



    }









}





















