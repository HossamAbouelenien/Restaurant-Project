namespace RMS.Services.Services.AiServices.NutritionServices.Options
{
    public class OpenAiOptions
    {
        public const string SectionName = "OpenAI";

        public string ApiKey { get; set; } = string.Empty;
        public string Model { get; set; } = "gpt-4o-mini";
        public string BaseUrl { get; set; } = "https://api.groq.com/openai/v1/";
    }
}
