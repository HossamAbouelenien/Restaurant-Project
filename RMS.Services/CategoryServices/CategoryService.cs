using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RMS.ServicesAbstraction.ICategoriesService;
using RMS.Shared.DTOs.CategoryDTOs;

namespace RMS.Services.CategoryServices
{
    public class CategoryService : ICategoriesService
    {
        public Task<IEnumerable<CategoryDTO>> GetAllCategoriesAsync()
        {
           
        }
    }
}
