using RMS.Shared;
using RMS.Shared.DTOs.MenuItemDTOs;
using RMS.Shared.DTOs.MenuItemsDTOs;
using RMS.Shared.QueryParams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RMS;
namespace RMS.ServicesAbstraction
{
    public interface IMenuItemService
    {
        Task<PaginatedResult<MenuItemDTO>> GetAllMenuItemsAsync(MenuItemQueryParams queryParams);
        Task<MenuItemDetailsDTO?> GetMenuItemByIdAsync(int id);
        Task<MenuItemDetailsDTO> CreateMenuItemAsync(CreateMenuItemDTO dto);
        Task<MenuItemDetailsDTO> UpdateMenuItemAsync(int id, UpdateMenuItemDTO dto);
        public Task ToggleAvailabilityAsync(int id);
        public Task DeleteMenuItemAsync(int id);
        public Task<IEnumerable<MenuItemDTO>> GetPopularMenuItemsAsync(int limit);
        public Task<MenuItemsStatsDTO> GetStatsAsync();
    }
}
