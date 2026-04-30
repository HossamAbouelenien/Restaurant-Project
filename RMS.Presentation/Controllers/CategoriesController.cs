using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<CategoriesController> _logger;

        public CategoriesController(ICategoryService categoryService, ILogger<CategoriesController> logger)
        {
            this._categoryService = categoryService;
            this._logger = logger;
        }

        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetAllCategories()
        {
            _logger.LogInformation("GetAllCategories request started");
            var Categories = await _categoryService.GetAllCategoriesAsync();
            return Ok(Categories);
        }

       
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDTO>> GetCategoryById(int id)
        {
            _logger.LogInformation("GetCategoryById request started");
            var Category = await _categoryService.GetCategoryByIdAsync(id);
            return Ok(Category);
        }

        [Authorize(Roles = SD.Role_Admin)]
        [HttpPost]
        public async Task<ActionResult<CategoryDTO>> AddCategory(CreateCategoryDTO DTO)
        {
            _logger.LogInformation("AddCategory request started");
            var Category = await _categoryService.AddCategoryAsync(DTO);
            return Ok(Category);
        }

        [Authorize(Roles = SD.Role_Admin)]
        [HttpPut("{id}")]
        public async Task<ActionResult<CategoryDTO>> UpdateCategory(int id, UpdateCategoryDTO DTO)
        {
            _logger.LogInformation("UpdateCategory request started");
            var result = await _categoryService.UpdateCategoryAsync(id, DTO);  

            return Ok(result);
        }

        [Authorize(Roles = SD.Role_Admin)]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCategory(int id)
        {
            _logger.LogInformation("DeleteCategory request started");
            await _categoryService.DeleteCategoryAsync(id);

                return NoContent();
            
           


        }

    }
}
