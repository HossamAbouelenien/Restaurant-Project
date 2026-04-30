using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMS.ServicesAbstraction.IServices.ITableServices;
using RMS.Shared.DTOs.TableDTOs;
using RMS.Shared.DTOs.Utility;
using RMS.Shared.QueryParams;

namespace RMS.Presentation.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class TableOrdersController : ControllerBase
    {
        private readonly ITableService _tableService;

        public TableOrdersController(ITableService tableService)
        {
            _tableService = tableService;
        }

        [Authorize(Roles = SD.Role_Admin + "" + SD.Role_Waiter)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TableOrderDTO>>> GetAllTableOrders([FromQuery] TableOrderQueryParams queryParams)
        {
            var tableOrders = await _tableService.GetAllTableOrdersAsync(queryParams);
            return Ok(tableOrders);
        }

        [Authorize(Roles = SD.Role_Admin + "" + SD.Role_Cashier)]
        [HttpPatch("{id}/complete")]
        public async Task<ActionResult<TableOrderDTO>> CompleteTableOrder(int id)
        {
            var result = await _tableService.CompleteTableOrderAsync(id);
            return Ok(result);
        }


    }

}
