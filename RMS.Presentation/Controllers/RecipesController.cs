using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RMS.ServicesAbstraction.IServices.IRecipeServices;
using RMS.Shared;
using RMS.Shared.DTOs.RecipeDTOs;
using RMS.Shared.DTOs.Utility;
using RMS.Shared.QueryParams;

namespace RMS.Presentation.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class RecipesController : ControllerBase
    {
        private readonly IRecipeService _recipeService;
        private readonly ILogger<RecipesController> _logger;

        public RecipesController(IRecipeService recipeService, ILogger<RecipesController> logger)
        {
            _recipeService = recipeService;
            _logger = logger;
        }

        [Authorize(Roles = SD.Role_Admin + "" + SD.Role_Chef)]
        [HttpGet]
        public async Task<ActionResult<PaginatedResult<RecipesListDTO>>> GetAllRecipes([FromQuery] RecipesQueryParams queryParams)
        {
            _logger.LogInformation("GetAllRecipes request started");
            var result = await _recipeService.GetAllRecipesAsync(queryParams);
            return Ok(result);
        }

        [Authorize(Roles = SD.Role_Admin)]
        [HttpPost]
        public async Task<ActionResult<RecipesListDTO>> AddRecipeToMenuItem([FromBody] AddRecipeToMenuItemDTO dto)
        {
            _logger.LogInformation("AddRecipeToMenuItem request started");
            var addedRecipe = await _recipeService.AddRecipeToMenuItemAsync(dto);
                return Ok(addedRecipe);
           
        }

        [Authorize(Roles = SD.Role_Admin)]
        [HttpPut("{id}")]
        public async Task<ActionResult<RecipesListDTO>> UpdateRecipeQuantity(int id, [FromBody] UpdateRecipeQuantityDTO dto)
        {
            _logger.LogInformation("UpdateRecipeQuantity request started");
            var updatedRecipe = await _recipeService.UpdateRecipeQuantityRequiredAsync(id, dto);
                return Ok(updatedRecipe);
           
        }
        [Authorize(Roles = SD.Role_Admin)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRecipe(int id)
        {
            _logger.LogInformation("DeleteRecipe request started");
            await _recipeService.DeleteRecipeAsync(id);

            return Ok();
        }
    }
}
