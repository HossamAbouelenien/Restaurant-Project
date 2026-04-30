using RMS.Shared.DTOs.AiDTOs;

namespace RMS.ServicesAbstraction.IServices.IAiServices
{
    public interface IAiRecipeService
    {
        Task<SuggestResponseDTO> SuggestRecipesAsync(SuggestRequestDTO request);
        Task SyncRecipesToAiAsync();
    }
}
