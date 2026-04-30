using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RMS.Services.Exceptions;
using RMS.Services.Services.AiServices.NutritionServices.Models;
using RMS.Services.Services.AiServices.NutritionServices.Options;
using RMS.ServicesAbstraction.IServices.IAiServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RMS.Services.Services.AiServices.NutritionServices
{
    public class NutritionAiService : INutritionAiService
    {
        private readonly HttpClient _httpClient;
        private readonly OpenAiOptions _options;
        private readonly ILogger<NutritionAiService> _logger;

        public NutritionAiService(
            HttpClient httpClient,
            IOptions<OpenAiOptions> options,
            ILogger<NutritionAiService> logger)
        {
            _httpClient = httpClient;
            _options = options.Value;
            _logger = logger;
        }

        public async Task<string> GetNutritionJsonAsync(string prompt, CancellationToken cancellationToken = default)
        {
            var requestBody = new OpenAiChatRequest
            {
                Model = _options.Model, // e.g. "gpt-4o-mini"
                Temperature = 0,        // deterministic output — critical for JSON
                Messages = new List<OpenAiMessage>
            {
                new()
                {
                    Role = "system",
                    Content = "You are a nutrition expert. Always respond with strict valid JSON only. No markdown. No explanations."
                },
                new()
                {
                    Role = "user",
                    Content = prompt
                }
            },
                ResponseFormat = new ResponseFormat { Type = "json_object" } // Force JSON mode
            };

            var json = JsonSerializer.Serialize(requestBody, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                PropertyNameCaseInsensitive = true
            });

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            

            _logger.LogDebug("Calling OpenAI API with model {Model}", _options.Model);

            var response = await _httpClient.PostAsync($"{_options.BaseUrl}chat/completions",
                content,
                cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync(cancellationToken);
                _logger.LogError("OpenAI API error {StatusCode}: {Error}", response.StatusCode, error);
                throw new AiServiceException(response.StatusCode.ToString());
            }

            var responseJson = await response.Content.ReadAsStringAsync(cancellationToken);
            var parsed = JsonSerializer.Deserialize<OpenAiChatResponse>(responseJson,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            var rawContent = parsed?.Choices?.FirstOrDefault()?.Message?.Content;

            if (rawContent is null)
            {
                _logger.LogError("OpenAI returned empty content");
                throw new AiEmptyResponseException();
            }
                

            _logger.LogDebug("Raw AI response received ({Length} chars)", rawContent.Length);

            return rawContent;
        }
    }
}
