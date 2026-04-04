using Microsoft.AspNetCore.Mvc;
using RMS.ServicesAbstraction;
using RMS.Shared.DTOs.BranchStockDTOs;
using RMS.Shared.QueryParams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Presentation.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class BranchStockController : ControllerBase
    {
        private readonly IBranchStockService _branchStockService;

        public BranchStockController(IBranchStockService branchStockService)
        {
            _branchStockService = branchStockService;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BranchStockDTO>>> GetAllBranchStock([FromQuery]BrandStockQueryParams queryParams)
        {
            var BranchStocks = await _branchStockService.GetAllBranchStockAsync(queryParams);
            return Ok(BranchStocks);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<BranchStockDTO>> GetBranchStock(int id)
        {
            var BranchStock = await _branchStockService.GetBranchStockAsync(id);
            return Ok(BranchStock);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<BranchStockDTO>> UpdateBranchStock(int id, UpdateBranchStockDTO updateBranchStock)
        {
            var BranchStock = await _branchStockService.UpdateBranchStockAsync(id, updateBranchStock);
            return Ok(BranchStock);
        }

    }
}
