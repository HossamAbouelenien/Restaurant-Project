using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.ServicesAbstraction.IAiServices
{
    public interface INutritionAiService
    {
        
        // Sends the nutrition prompt to the AI and returns a strict JSON string.
        Task<string> GetNutritionJsonAsync(string prompt, CancellationToken cancellationToken = default);
    }
}
