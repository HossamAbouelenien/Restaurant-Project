using RMS.Shared;
using RMS.Shared.DTOs.MenuItemDTOs;
using RMS.Shared.DTOs.RecipeDTOs;
using RMS.Shared.QueryParams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.ServicesAbstraction
{
    public interface IRecipeService
    {
        Task<PaginatedResult<RecipesListDTO>>GetAllRecipesAsync(RecipesQueryParams queryParams);
        Task<RecipesListDTO> AddRecipeToMenuItemAsync(AddRecipeToMenuItemDTO dto);
        Task<RecipesListDTO> UpdateRecipeQuantityRequiredAsync(int recipeId, UpdateRecipeQuantityDTO dto);
        Task DeleteRecipeAsync(int id);
    }
}
