using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RMS.Shared.DTOs.CategoryDTOs;

namespace RMS.ServicesAbstraction.ICategoriesService
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDTO>> GetAllCategoriesAsync();
        Task<CategoryDTO?> GetCategoryByIdAsync(int id);
        Task<int> AddCategoryAsync(CreateCategoryDTO DTO);

    }
}
