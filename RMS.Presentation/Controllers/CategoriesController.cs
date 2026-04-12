using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RMS.ServicesAbstraction.ICategoriesService;
using RMS.Shared.DTOs.CategoryDTOs;

namespace RMS.Presentation.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            this._categoryService = categoryService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetAllCategories()
        {
            var Categories = await _categoryService.GetAllCategoriesAsync();
            return Ok(Categories);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDTO>> GetCategoryById(int id)
        {
            var Category = await _categoryService.GetCategoryByIdAsync(id);
            return Ok(Category);
        }

        [HttpPost]
        public async Task<ActionResult<CategoryDTO>> AddCategory(CreateCategoryDTO DTO)
        {
            var Category = await _categoryService.AddCategoryAsync(DTO);
            return Ok(Category);
        }






    }
}
