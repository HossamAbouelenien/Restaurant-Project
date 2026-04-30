using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RMS.ServicesAbstraction.IServices.IAiServices;
using RMS.Shared.DTOs.AiDTOs;
using RMS.Shared.DTOs.Utility;

namespace RMS.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AiRecipeController : ControllerBase
    {
        private readonly IAiRecipeService _aiRecipeService;
        private readonly ILogger<AiRecipeController> _logger;

        public AiRecipeController(IAiRecipeService aiRecipeService, ILogger<AiRecipeController> logger)
        {
            _aiRecipeService = aiRecipeService;
            _logger = logger;
        }

        [HttpPost("suggest")]
        public async Task<ActionResult<SuggestResponseDTO>> Suggest([FromBody] SuggestRequestDTO request)
        {
            _logger.LogInformation("AI Recipe Suggest request started");
            var result = await _aiRecipeService.SuggestRecipesAsync(request);
            return Ok(result);
        }

        //[Authorize(Roles = SD.Role_Admin)]
        [HttpPost("sync")]
        public async Task<IActionResult> Sync()
        {
            _logger.LogInformation("AI Sync request started");
            await _aiRecipeService.SyncRecipesToAiAsync();
            return Ok(new { message = "Recipes synced to AI ✅" });
        }
    }
}
