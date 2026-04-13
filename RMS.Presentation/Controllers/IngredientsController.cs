using Microsoft.AspNetCore.Mvc;
using RMS.ServicesAbstraction;
using RMS.Shared.DTOs.IngredientDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        [HttpGet]
        public async Task<IActionResult> GetAllIngredients(
          [FromQuery] int pageIndex = 1,
          [FromQuery] int pageSize = 10)
        {
            var result = await _ingredientService.GetAllIngredientsAsync(pageIndex, pageSize);
            return Ok(result);
        }

        [HttpGet("{id}")]

        public async Task<IActionResult> GetIngredientById(int id)
        {
            var ingredient = await _ingredientService.GetIngredientByIdAsync(id);
            if (ingredient == null)
                return NotFound();
            return Ok(ingredient);

        }
        [HttpPost]
        public async Task<IActionResult> AddIngredient(CreateIngredientDTO NewIngredient)
        {
            var Ingredient = await _ingredientService.CreateIngredientAsync(NewIngredient);
            return Ok(Ingredient);

        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateIngredient(int id, [FromBody] CreateIngredientDTO UpdateIngredient)
        {
            var Ingredient = await _ingredientService.UpdateIngredientAsync(id, UpdateIngredient);
            return Ok(Ingredient);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIngredient(int id)
        {
            await _ingredientService.DeleteIngredientAsync(id);
            return NoContent();

        }
    }
}
