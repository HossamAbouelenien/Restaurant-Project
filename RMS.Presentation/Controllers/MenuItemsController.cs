using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RMS.ServicesAbstraction.IServices.IMenuItemServices;
using RMS.Shared;
using RMS.Shared.DTOs.MenuItemDTOs;
using RMS.Shared.DTOs.MenuItemsDTOs;
using RMS.Shared.DTOs.Utility;
using RMS.Shared.QueryParams;

namespace RMS.Presentation.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class MenuItemsController : ControllerBase
    {
        private readonly IMenuItemService _menuItemService;
        private readonly ILogger<MenuItemsController> _logger;

        public MenuItemsController(IMenuItemService menuItemService, ILogger<MenuItemsController> logger)
        {
            _menuItemService = menuItemService;
            _logger = logger;
        }

       
        [HttpGet]
        //[Cache]
        public async Task<ActionResult<PaginatedResult<MenuItemDTO>>> GetAllMenuItems([FromQuery] MenuItemQueryParams queryParams)
        {
            _logger.LogInformation("GetAllMenuItems request started");
            var result = await _menuItemService.GetAllMenuItemsAsync(queryParams);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MenuItemDetailsDTO>> GetMenuItemById(int id)
        {
            _logger.LogInformation("GetMenuItemById request started");
            var result = await _menuItemService.GetMenuItemByIdAsync(id);
            if (result is null)
            {
                _logger.LogWarning("MenuItem with id {Id} not found", id);
                return NotFound();
            }
                
            return Ok(result);
        }

        //[Authorize(Roles = SD.Role_Admin)]
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<MenuItemDetailsDTO>> CreateMenuItem([FromForm] CreateMenuItemDTO dto)
        {
            _logger.LogInformation("CreateMenuItem request started");
            if (!ModelState.IsValid) return BadRequest(ModelState);

           
                var result = await _menuItemService.CreateMenuItemAsync(dto);
                return CreatedAtAction(nameof(GetMenuItemById), new { id = result.Id }, result);
            
           
        }

        //[Authorize(Roles = SD.Role_Admin)]
        [HttpPut("{id}")]
        public async Task<ActionResult<MenuItemDetailsDTO>> UpdateMenuItem(int id, [FromForm] UpdateMenuItemDTO dto)
        {
                _logger.LogInformation("UpdateMenuItem request started");
                var result = await _menuItemService.UpdateMenuItemAsync(id, dto);
                return Ok(result);
            
           
        }

        //[Authorize(Roles = SD.Role_Admin + "" + SD.Role_Chef)]
        [HttpPatch("{id}/toggle-availability")]
        public async Task<ActionResult> ToggleAvailability(int id)
        {
            _logger.LogInformation("ToggleAvailability request started");
            await _menuItemService.ToggleAvailabilityAsync(id);
            return Ok(); 
        }

        //[Authorize(Roles = SD.Role_Admin)]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMenuItem(int id)
        {
            _logger.LogInformation("DeleteMenuItem request started");
            await _menuItemService.DeleteMenuItemAsync(id);
                return Ok();
          
        }

        [HttpGet("popular")]
        public async Task<IActionResult> GetPopular([FromQuery] int? branchId)
        {
            _logger.LogInformation("GetPopularMenuItems request started");
            var result = await _menuItemService.GetPopularMenuItemsAsync(4, branchId);
            return Ok(result);
        }

        [HttpGet("stats")]
        public async Task<ActionResult<MenuItemsStatsDTO>> GetStats()
        {
            _logger.LogInformation("GetMenuItemsStats request started");
            var stats = await _menuItemService.GetStatsAsync();
            return Ok(stats);
        }
    }





}
















