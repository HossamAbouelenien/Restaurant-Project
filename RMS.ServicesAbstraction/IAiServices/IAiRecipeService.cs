using RMS.Shared.DTOs.AiDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.ServicesAbstraction.IAiServices
{
    public interface IAiRecipeService
    {
        Task<SuggestResponseDTO> SuggestRecipesAsync(SuggestRequestDTO request);
        Task SyncRecipesToAiAsync();
    }
}
