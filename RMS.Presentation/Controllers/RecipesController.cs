using Microsoft.AspNetCore.Mvc;
using RMS.Domain.Entities;
using RMS.ServicesAbstraction.IServices.IRecipeServices;
using RMS.Shared;
using RMS.Shared.DTOs.MenuItemsDTOs;
using RMS.Shared.DTOs.RecipeDTOs;
using RMS.Shared.QueryParams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        [HttpGet]
        public async Task<ActionResult<PaginatedResult<RecipesListDTO>>> GetAllRecipes([FromQuery] RecipesQueryParams queryParams)
        {
            var result = await _recipeService.GetAllRecipesAsync(queryParams);
            return Ok(result);
        }


        [HttpPost]
        public async Task<ActionResult<RecipesListDTO>> AddRecipeToMenuItem([FromBody] AddRecipeToMenuItemDTO dto)
        {
            
                var addedRecipe = await _recipeService.AddRecipeToMenuItemAsync(dto);
                return Ok(addedRecipe);
           
        }


        [HttpPut("{id}")]
        public async Task<ActionResult<RecipesListDTO>> UpdateRecipeQuantity(int id, [FromBody] UpdateRecipeQuantityDTO dto)
        {
            
                var updatedRecipe = await _recipeService.UpdateRecipeQuantityRequiredAsync(id, dto);
                return Ok(updatedRecipe);
           
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRecipe(int id)
        {
            await _recipeService.DeleteRecipeAsync(id);

            return Ok();
        }
    }
}
