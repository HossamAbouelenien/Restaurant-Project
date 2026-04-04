using Microsoft.AspNetCore.Mvc;
using RMS.ServicesAbstraction;
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
        public async Task<IActionResult> GetAllIngredients()
        { 
            var ingredients = await _ingredientService.GetAllIngredientsAsync();
            return Ok(ingredients);
        }

    }
}
