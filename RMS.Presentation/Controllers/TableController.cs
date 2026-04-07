using Microsoft.AspNetCore.Mvc;
using RMS.ServicesAbstraction;
using RMS.Shared.DTOs.TableDTOs;
using RMS.Shared.QueryParams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TableController : ControllerBase
    {
        private readonly ITableService _tableService;

        public TableController(ITableService tableService)
        {
            _tableService = tableService;
        }

        [HttpPost]
        public async Task<ActionResult<TableDTO>> CreateTable(CreateTableDTO createTable)
        {
            var table = await _tableService.CreateTableAsync(createTable);
            return Ok(table);
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TableDTO>>> GetAllTables([FromQuery] TableQueryParams queryParams)
        {
            var tables = await _tableService.GetAllTablesAsync(queryParams);
            return Ok(tables);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<TableDTO>> GetTableById(int id)
        {
            var table = await _tableService.GetTableByIdAsync(id);
            if (table == null) return NotFound();
            return Ok(table);
        }
        [HttpPatch("{id}")]
        public async Task<ActionResult<TableDTO>>UpdateTable(int id,UpdateTableDTO dto) { 
            var table= await _tableService.UpdateTableAsync(id, dto);
            return Ok(table);
        }

        [HttpDelete("{id}")]
       
        public async Task<ActionResult> DeleteTable(int id)
        {
            await _tableService.DeleteTableAsync(id);
            return Ok();
        }
        [HttpPatch("{id}/status")]
        public async Task<ActionResult<TableDTO>> UpdateTableStatus(int id, UpdateTableStatusDTO dto)
        {
            var table = await _tableService.UpdateTableStatusAsync(id, dto);
            return Ok(table);
        }
    }
}
