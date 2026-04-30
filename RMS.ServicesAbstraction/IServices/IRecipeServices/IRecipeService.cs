using RMS.Shared;
using RMS.Shared.DTOs.RecipeDTOs;
using RMS.Shared.QueryParams;

namespace RMS.ServicesAbstraction.IServices.IRecipeServices
{
    public interface IRecipeService
    {
        Task<PaginatedResult<RecipesListDTO>>GetAllRecipesAsync(RecipesQueryParams queryParams);
        Task<RecipesListDTO> AddRecipeToMenuItemAsync(AddRecipeToMenuItemDTO dto);
        Task<RecipesListDTO> UpdateRecipeQuantityRequiredAsync(int recipeId, UpdateRecipeQuantityDTO dto);
        Task DeleteRecipeAsync(int id);
    }
}
