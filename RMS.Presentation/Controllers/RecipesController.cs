using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        public RecipesController(IRecipeService recipeService)
        {
            _recipeService = recipeService;
        }

        [Authorize(Roles = SD.Role_Admin + "" + SD.Role_Chef)]
        [HttpGet]
        public async Task<ActionResult<PaginatedResult<RecipesListDTO>>> GetAllRecipes([FromQuery] RecipesQueryParams queryParams)
        {
            var result = await _recipeService.GetAllRecipesAsync(queryParams);
            return Ok(result);
        }

        [Authorize(Roles = SD.Role_Admin)]
        [HttpPost]
        public async Task<ActionResult<RecipesListDTO>> AddRecipeToMenuItem([FromBody] AddRecipeToMenuItemDTO dto)
        {
            
                var addedRecipe = await _recipeService.AddRecipeToMenuItemAsync(dto);
                return Ok(addedRecipe);
           
        }

        [Authorize(Roles = SD.Role_Admin)]
        [HttpPut("{id}")]
        public async Task<ActionResult<RecipesListDTO>> UpdateRecipeQuantity(int id, [FromBody] UpdateRecipeQuantityDTO dto)
        {
            
                var updatedRecipe = await _recipeService.UpdateRecipeQuantityRequiredAsync(id, dto);
                return Ok(updatedRecipe);
           
        }
        [Authorize(Roles = SD.Role_Admin)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRecipe(int id)
        {
            await _recipeService.DeleteRecipeAsync(id);

            return Ok();
        }
    }
}
