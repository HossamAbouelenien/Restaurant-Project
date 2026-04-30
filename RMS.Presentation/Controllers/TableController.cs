using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RMS.ServicesAbstraction.IServices.ITableServices;
using RMS.Shared.DTOs.TableDTOs;
using RMS.Shared.DTOs.Utility;
using RMS.Shared.QueryParams;

namespace RMS.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TableController : ControllerBase
    {
        private readonly ITableService _tableService;
        private readonly ILogger<TableController> _logger;

        public TableController(ITableService tableService, ILogger<TableController> logger)
        {
            _tableService = tableService;
            _logger = logger;
        }

        [Authorize(Roles = SD.Role_Admin)]
        [HttpPost]
        public async Task<ActionResult<TableDTO>> CreateTable(CreateTableDTO createTable)
        {
            _logger.LogInformation("CreateTable request started");
            var table = await _tableService.CreateTableAsync(createTable);
            return Ok(table);
        }

        [Authorize(Roles = SD.Role_Admin + "" + SD.Role_Cashier + "" + SD.Role_Waiter)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TableDTO>>> GetAllTables([FromQuery] TableQueryParams queryParams)
        {
            _logger.LogInformation("GetAllTables request started");
            var tables = await _tableService.GetAllTablesAsync(queryParams);
            return Ok(tables);
        }


        [Authorize(Roles = SD.Role_Admin + "" + SD.Role_Waiter)]
        [HttpGet("{id}")]
        public async Task<ActionResult<TableDTO>> GetTableById(int id)
        {
            _logger.LogInformation("GetTableById request started");
            var table = await _tableService.GetTableByIdAsync(id);
            return Ok(table);
        }


        [Authorize(Roles = SD.Role_Admin )]
        [HttpPatch("{id}")]
        public async Task<ActionResult<TableDTO>>UpdateTable(int id,UpdateTableDTO dto)
        {
            _logger.LogInformation("UpdateTable request started");
            var table= await _tableService.UpdateTableAsync(id, dto);
            return Ok(table);
        }

        [Authorize(Roles = SD.Role_Admin)]
        [HttpDelete("{id}")]
       
        public async Task<ActionResult> DeleteTable(int id)
        {
            _logger.LogInformation("DeleteTable request started");
            await _tableService.DeleteTableAsync(id);
            return Ok();
        }

        [Authorize(Roles = SD.Role_Admin + "" + SD.Role_Waiter)]
        [HttpPatch("{id}/status")]
        public async Task<ActionResult> UpdateTableStatus(int id)
        {
            _logger.LogInformation("ToggleTableStatus request started");
            await _tableService.ToggleTableStatusAsync(id);
            return Ok();
        }


    }
}


