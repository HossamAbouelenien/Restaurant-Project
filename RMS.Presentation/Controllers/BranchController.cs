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
        public async Task<ActionResult<BranchDTO>> GetBranch(int id)
        {
            var Branch = await _branchService.GetBranchByIdAsync(id);
            return Ok(Branch);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<UpdateBranchDTO>> UpdateBranch(int id, UpdateBranchDTO updateBranch)
        {
            var Branch = await _branchService.UpdateBranchAsync(id, updateBranch);
            return Ok(Branch);
        }
        [HttpPost]
        public async Task<ActionResult<CreateBranchDTO>> CreateBranch(CreateBranchDTO BranchDTO)
        {
            var Branch = await _branchService.CreateBranchAsync(BranchDTO);
             return Ok(Branch);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBranch(int id)
        {
            await _branchService.DeleteBranchAsync(id);
            return Ok();
        }
        [HttpPatch("{id}/toggle-status")]
        public async Task<ActionResult> ToggleBranchStatus(int id)
        {
            await _branchService.ToggleBranchStatusAsync(id);
            return Ok();
        }
    }
}
