using Microsoft.AspNetCore.Mvc;
using RMS.ServicesAbstraction;
using RMS.Shared.DTOs.BranchDTOs;
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
    public class BranchController : ControllerBase
    {
        private readonly IBranchService _branchService;
        public BranchController(IBranchService branchService)
        {
            _branchService = branchService;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BranchStockDTO>>> GetAllBranches()
        {
            var BranchStocks = await _branchService.GetAllBranchesAsync();
            return Ok(BranchStocks);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<BranchDTO>> GetBranchStock(int id)
        {
            var BranchStock = await _branchService.GetBranchByIdAsync(id);
            return Ok(BranchStock);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<BranchDTO>> UpdateBranchStock(int id, BranchDTO updateBranch)
        {
            var BranchStock = await _branchService.UpdateBranchAsync(id, updateBranch);
            return Ok(BranchStock);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBranchStock(int id)
        {
            await _branchService.DeleteBranchAsync(id);
            return Ok();
        }
    }
}
