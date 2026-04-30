using Microsoft.AspNetCore.Mvc;
using RMS.ServicesAbstraction.IServices.IMenuItemServices;
using RMS.Shared;
using RMS.Shared.DTOs.MenuItemDTOs;
using RMS.Shared.DTOs.MenuItemsDTOs;
using RMS.Shared.QueryParams;

namespace RMS.Presentation.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class MenuItemsController : ControllerBase
    {
        private readonly IMenuItemService _menuItemService;

        public MenuItemsController(IMenuItemService menuItemService)
        {
            _menuItemService = menuItemService;
        }

        //[Authorize]
        [HttpGet]
        //[Cache]
        public async Task<ActionResult<PaginatedResult<MenuItemDTO>>> GetAllMenuItems([FromQuery] MenuItemQueryParams queryParams)
        {
            var result = await _menuItemService.GetAllMenuItemsAsync(queryParams);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MenuItemDetailsDTO>> GetMenuItemById(int id)
        {
            var result = await _menuItemService.GetMenuItemByIdAsync(id);
            if (result is null) return NotFound();
            return Ok(result);
        }


        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<MenuItemDetailsDTO>> CreateMenuItem([FromForm] CreateMenuItemDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

           
                var result = await _menuItemService.CreateMenuItemAsync(dto);
                return CreatedAtAction(nameof(GetMenuItemById), new { id = result.Id }, result);
            
           
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<MenuItemDetailsDTO>> UpdateMenuItem(int id, [FromForm] UpdateMenuItemDTO dto)
        {
           
                var result = await _menuItemService.UpdateMenuItemAsync(id, dto);
                return Ok(result);
            
           
        }


        [HttpPatch("{id}/toggle-availability")]
        public async Task<ActionResult> ToggleAvailability(int id)
        {
            await _menuItemService.ToggleAvailabilityAsync(id);
            return Ok(); 
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMenuItem(int id)
        {
           
                await _menuItemService.DeleteMenuItemAsync(id);
                return Ok();
          
        }

        [HttpGet("popular")]
        public async Task<IActionResult> GetPopular([FromQuery] int? branchId)
        {
            var result = await _menuItemService.GetPopularMenuItemsAsync(4, branchId);
            return Ok(result);
        }

        [HttpGet("stats")]
        public async Task<ActionResult<MenuItemsStatsDTO>> GetStats()
        {
            var stats = await _menuItemService.GetStatsAsync();
            return Ok(stats);
        }
    }





}
















