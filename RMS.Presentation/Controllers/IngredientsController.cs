using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<IngredientsController> _logger;
        public IngredientsController(IIngredientService ingredientService, ILogger<IngredientsController> logger)
        {
            _ingredientService = ingredientService;
            _logger = logger;
        }

        [Authorize(Roles = SD.Role_Admin + "" + SD.Role_Chef)]
        [HttpGet]
        public async Task<IActionResult> GetAllIngredients(
          [FromQuery] int pageIndex = 1,
          [FromQuery] int pageSize = 10)
        {
            _logger.LogInformation("GetAllIngredients request started");
            var result = await _ingredientService.GetAllIngredientsAsync(pageIndex, pageSize);
            return Ok(result);
        }

        [Authorize(Roles = SD.Role_Admin + "" + SD.Role_Chef)]
        [HttpGet("{id}")]

        public async Task<IActionResult> GetIngredientById(int id)
        {
            _logger.LogInformation("GetIngredientById request started");
            var ingredient = await _ingredientService.GetIngredientByIdAsync(id);
            if (ingredient == null)
            {
                _logger.LogWarning("Ingredient with id {Id} not found", id);
                return NotFound();
            }
                
            return Ok(ingredient);

        }

        [Authorize(Roles = SD.Role_Admin + "" + SD.Role_Chef)]
        [HttpPost]
        public async Task<IActionResult> AddIngredient(CreateIngredientDTO NewIngredient)
        {
            _logger.LogInformation("AddIngredient request started");
            var Ingredient = await _ingredientService.CreateIngredientAsync(NewIngredient);
            return Ok(Ingredient);

        }

        [Authorize(Roles = SD.Role_Admin)]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateIngredient(int id, [FromBody] CreateIngredientDTO UpdateIngredient)
        {
            _logger.LogInformation("UpdateIngredient request started");
            var Ingredient = await _ingredientService.UpdateIngredientAsync(id, UpdateIngredient);
            return Ok(Ingredient);
        }

        [Authorize(Roles = SD.Role_Admin)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIngredient(int id)
        {
            _logger.LogInformation("DeleteIngredient request started");
            await _ingredientService.DeleteIngredientAsync(id);
            return NoContent();

        }
    }
}
