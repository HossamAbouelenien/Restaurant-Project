using RMS.Shared.DTOs.NutritionDTOs;

namespace RMS.ServicesAbstraction.IServices.IAiServices
{
    public interface INutritionService
    {
        Task<NutritionResponseDto> CalculateBasketNutritionAsync(string basketId, CancellationToken cancellationToken = default);
    }
}
