namespace RMS.ServicesAbstraction.IServices.IAiServices
{
    public interface INutritionAiService
    {
        
        // Sends the nutrition prompt to the AI and returns a strict JSON string.
        Task<string> GetNutritionJsonAsync(string prompt, CancellationToken cancellationToken = default);
    }
}
