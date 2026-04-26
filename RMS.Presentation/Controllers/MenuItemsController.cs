using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMS.Presentation.Attributes;
using RMS.ServicesAbstraction;
using RMS.Shared;
using RMS.Shared.DTOs.MenuItemDTOs;
using RMS.Shared.DTOs.MenuItemsDTOs;
using RMS.Shared.QueryParams;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            try
            {
                var result = await _menuItemService.CreateMenuItemAsync(dto);
                return CreatedAtAction(nameof(GetMenuItemById), new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<MenuItemDetailsDTO>> UpdateMenuItem(int id, [FromForm] UpdateMenuItemDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var result = await _menuItemService.UpdateMenuItemAsync(id, dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
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
            try
            {
                await _menuItemService.DeleteMenuItemAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("popular")]
        public async Task<IActionResult> GetPopular()
        {
            var result = await _menuItemService.GetPopularMenuItemsAsync(4);
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