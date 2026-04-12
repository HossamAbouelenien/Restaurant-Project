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

        [HttpPut("{id}")]
        public async Task<ActionResult<CategoryDTO>> UpdateCategory(int id, UpdateCategoryDTO DTO)
        {
            var result = await _categoryService.UpdateCategoryAsync(id, DTO);

            if (result == null)
            {
                return NotFound(new { message = "Category not found" });
            }

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCategory(int id)
        {
            try
            {
                await _categoryService.DeleteCategoryAsync(id);

                return NoContent();
            }
            catch (Exception ex)
            {
                if (ex.Message == "Category not found")
                    return NotFound(new { message = ex.Message });

                return BadRequest(new { message = ex.Message });
            }



        }

    }
}
