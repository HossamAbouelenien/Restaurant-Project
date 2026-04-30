using Microsoft.AspNetCore.Mvc;
using RMS.ServicesAbstraction.IServices.IBranchServices;
using RMS.Shared;
using RMS.Shared.DTOs.BranchDTOs;
using RMS.Shared.QueryParams;

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
        public async Task<ActionResult<IEnumerable<BranchDTO>>> GetAllBranches()
        {
            var BranchStocks = await _branchService.GetAllBranchesAsync();
            return Ok(BranchStocks);
        }



        [HttpGet("GetAllBranchesWithTables")]
        public async Task<ActionResult<PaginatedResult<GetBranchDTO>>> GetAllBranchesWithOrdersAndTablesAsync([FromQuery] BranchQueryParams param)
        {
            var result = await _branchService.GetAllBranchesWithOrdersAndTablesAsync(param);
            return Ok(result);
        }
     

        [HttpGet("{id:int}")]
        public async Task<ActionResult<GetBranchDTO>> GetBranchById(int id)
        {
            var result = await _branchService.GetBranchByIdAsync(id);
            return Ok(result);
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
