using RMS.Shared;
using RMS.Shared.DTOs.MenuItemDTOs;
using RMS.Shared.DTOs.MenuItemsDTOs;
using RMS.Shared.QueryParams;
namespace RMS.ServicesAbstraction.IServices.IMenuItemServices
{
    public interface IMenuItemService
    {
        Task<PaginatedResult<MenuItemDTO>> GetAllMenuItemsAsync(MenuItemQueryParams queryParams);
        Task<MenuItemDetailsDTO?> GetMenuItemByIdAsync(int id);
        Task<MenuItemDetailsDTO> CreateMenuItemAsync(CreateMenuItemDTO dto);
        Task<MenuItemDetailsDTO> UpdateMenuItemAsync(int id, UpdateMenuItemDTO dto);
        public Task ToggleAvailabilityAsync(int id);
        public Task DeleteMenuItemAsync(int id);
        public Task<IEnumerable<MenuItemDTO>> GetPopularMenuItemsAsync(int limit, int? branchId);
        public Task<MenuItemsStatsDTO> GetStatsAsync();
    }
}
