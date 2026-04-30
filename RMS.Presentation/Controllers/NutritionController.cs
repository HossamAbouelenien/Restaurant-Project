using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RMS.ServicesAbstraction.IServices.IAiServices;
using RMS.Shared.DTOs.NutritionDTOs;

namespace RMS.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NutritionController : ControllerBase
    {
        private readonly INutritionService _nutritionService;
        private readonly ILogger<NutritionController> _logger;

        public NutritionController(INutritionService nutritionService, ILogger<NutritionController> logger)
        {
            _nutritionService = nutritionService;
            _logger = logger;
        }

        /// <summary>
        /// Calculates nutritional information for all items in the basket using AI.
        /// </summary>
        /// <param name="basketId">The Redis basket identifier</param>
        [HttpPost("{basketId}/nutrition")]
        [ProducesResponseType(typeof(NutritionResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status502BadGateway)]
        public async Task<IActionResult> CalculateNutrition([FromRoute] string basketId,
                    CancellationToken cancellationToken)
     
        {
            _logger.LogInformation("CalculateNutrition request started");
            try
            {
                var result = await _nutritionService
                    .CalculateBasketNutritionAsync(basketId, cancellationToken);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { ex.Message });
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "AI service failed for basket {BasketId}", basketId);
                return StatusCode(502, new { Message = "AI service is currently unavailable." });
            }
        }
    }
}
