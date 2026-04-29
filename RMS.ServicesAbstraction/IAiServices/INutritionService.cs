using RMS.Shared.DTOs.NutritionDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.ServicesAbstraction.IAiServices
{
    public interface INutritionService
    {
        Task<NutritionResponseDto> CalculateBasketNutritionAsync(string basketId, CancellationToken cancellationToken = default);
    }
}
