using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RMS.ServicesAbstraction.IServices.IBranchStockServices;
using RMS.Shared.DTOs.BranchStockDTOs;
using RMS.Shared.DTOs.Utility;
using RMS.Shared.QueryParams;

namespace RMS.Presentation.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class BranchStockController : ControllerBase
    {
        private readonly IBranchStockService _branchStockService;
        private readonly ILogger<BranchStockController> _logger;

        public BranchStockController(IBranchStockService branchStockService, ILogger<BranchStockController> loggerogger)
        {
            _branchStockService = branchStockService;
            _logger = loggerogger;
        }


        [Authorize(Roles = SD.Role_Admin + "" + SD.Role_Chef)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BranchStockDTO>>> GetAllBranchStock([FromQuery]BrandStockQueryParams queryParams)
        {
            _logger.LogInformation("GetAllBranchStock request started");
            var BranchStocks = await _branchStockService.GetAllBranchStockAsync(queryParams);
            return Ok(BranchStocks);
        }

        [Authorize(Roles = SD.Role_Admin + "" + SD.Role_Chef)]
        [HttpGet("{id}")]
        public async Task<ActionResult<BranchStockDTO>> GetBranchStock(int id)
        {
            _logger.LogInformation("GetBranchStock request started");
            var BranchStock = await _branchStockService.GetBranchStockAsync(id);
            return Ok(BranchStock);
        }

        [Authorize(Roles = SD.Role_Admin + "" + SD.Role_Chef)]
        [HttpPatch("{id}")]
        public async Task<ActionResult<BranchStockDTO>> UpdateBranchStock(int id, UpdateBranchStockDTO updateBranchStock)
        {
            _logger.LogInformation("UpdateBranchStock request started");
            var BranchStock = await _branchStockService.UpdateBranchStockAsync(id, updateBranchStock);
            return Ok(BranchStock);
        }



    }
}
