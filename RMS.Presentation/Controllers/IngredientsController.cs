using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMS.ServicesAbstraction.IServices.IIngredientServices;
using RMS.Shared.DTOs.IngredientDTOs;
using RMS.Shared.DTOs.Utility;

namespace RMS.Presentation.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]

    public class IngredientsController : ControllerBase
    {
        readonly IIngredientService _ingredientService;
        public IngredientsController(IIngredientService ingredientService)
        {
            _ingredientService = ingredientService;
        }

        [Authorize(Roles = SD.Role_Admin + "" + SD.Role_Chef)]
        [HttpGet]
        public async Task<IActionResult> GetAllIngredients(
          [FromQuery] int pageIndex = 1,
          [FromQuery] int pageSize = 10)
        {
            var result = await _ingredientService.GetAllIngredientsAsync(pageIndex, pageSize);
            return Ok(result);
        }

        [Authorize(Roles = SD.Role_Admin + "" + SD.Role_Chef)]
        [HttpGet("{id}")]

        public async Task<IActionResult> GetIngredientById(int id)
        {
            var ingredient = await _ingredientService.GetIngredientByIdAsync(id);
            if (ingredient == null)
                return NotFound();
            return Ok(ingredient);

        }

        [Authorize(Roles = SD.Role_Admin + "" + SD.Role_Chef)]
        [HttpPost]
        public async Task<IActionResult> AddIngredient(CreateIngredientDTO NewIngredient)
        {
            var Ingredient = await _ingredientService.CreateIngredientAsync(NewIngredient);
            return Ok(Ingredient);

        }

        [Authorize(Roles = SD.Role_Admin)]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateIngredient(int id, [FromBody] CreateIngredientDTO UpdateIngredient)
        {
            var Ingredient = await _ingredientService.UpdateIngredientAsync(id, UpdateIngredient);
            return Ok(Ingredient);
        }

        [Authorize(Roles = SD.Role_Admin)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIngredient(int id)
        {
            await _ingredientService.DeleteIngredientAsync(id);
            return NoContent();

        }
    }
}
