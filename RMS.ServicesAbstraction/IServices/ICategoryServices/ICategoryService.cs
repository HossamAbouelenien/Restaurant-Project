using RMS.Shared.DTOs.CategoryDTOs;

namespace RMS.ServicesAbstraction.IServices.ICategoryServices
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDTO>> GetAllCategoriesAsync();
        Task<CategoryDTO?> GetCategoryByIdAsync(int id);
        Task<CategoryDTO> AddCategoryAsync(CreateCategoryDTO DTO);
        Task<CategoryDTO> UpdateCategoryAsync(int id, UpdateCategoryDTO DTO);

        Task DeleteCategoryAsync(int id);
    }
}
