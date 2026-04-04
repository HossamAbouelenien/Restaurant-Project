using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    public class MenuItemController : ControllerBase
    {
        private readonly IMenuItemService _menuItemService;

        public MenuItemController(IMenuItemService menuItemService)
        {
            _menuItemService = menuItemService;
        }

        //[Authorize]
        [HttpGet]
       
        public async Task<ActionResult<PaginatedResult<MenuItemDTO>>> GetAllMenuItems ([FromQuery] MenuItemQueryParams queryParams)
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
    }
}