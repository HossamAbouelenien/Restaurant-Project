using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        public AiRecipeController(IAiRecipeService aiRecipeService)
        {
            _aiRecipeService = aiRecipeService;
        }

        [HttpPost("suggest")]
        public async Task<ActionResult<SuggestResponseDTO>> Suggest([FromBody] SuggestRequestDTO request)
        {
            var result = await _aiRecipeService.SuggestRecipesAsync(request);
            return Ok(result);
        }

        [Authorize(Roles = SD.Role_Admin)]
        [HttpPost("sync")]
        public async Task<IActionResult> Sync()
        {
            await _aiRecipeService.SyncRecipesToAiAsync();
            return Ok(new { message = "Recipes synced to AI ✅" });
        }
    }
}
