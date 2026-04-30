using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMS.ServicesAbstraction.IServices.ICategoryServices;
using RMS.Shared.DTOs.CategoryDTOs;
using RMS.Shared.DTOs.Utility;

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

        [Authorize(Roles = SD.Role_Admin)]
        [HttpPost]
        public async Task<ActionResult<CategoryDTO>> AddCategory(CreateCategoryDTO DTO)
        {
            var Category = await _categoryService.AddCategoryAsync(DTO);
            return Ok(Category);
        }

        [Authorize(Roles = SD.Role_Admin)]
        [HttpPut("{id}")]
        public async Task<ActionResult<CategoryDTO>> UpdateCategory(int id, UpdateCategoryDTO DTO)
        {
            var result = await _categoryService.UpdateCategoryAsync(id, DTO);  

            return Ok(result);
        }

        [Authorize(Roles = SD.Role_Admin)]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCategory(int id)
        {
            
                await _categoryService.DeleteCategoryAsync(id);

                return NoContent();
            
           


        }

    }
}
