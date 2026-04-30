using RMS.Shared;
using RMS.Shared.DTOs.IngredientDTOs;

namespace RMS.ServicesAbstraction.IServices.IIngredientServices
{
    public interface IIngredientService
    {
        Task<PaginatedResult<IngredientDTO>> GetAllIngredientsAsync(int pageIndex, int pageSize);
        Task<IngredientDTO> GetIngredientByIdAsync(int id);

        Task<IngredientDTO> CreateIngredientAsync(CreateIngredientDTO createIngredientDTO);

        Task<IngredientDTO> UpdateIngredientAsync(int id, CreateIngredientDTO updateIngredientDTO);

        Task DeleteIngredientAsync(int id);



    }
}
