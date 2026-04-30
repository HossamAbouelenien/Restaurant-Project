using RMS.Domain.Contracts;
using RMS.ServicesAbstraction.IServices.ICacheServices;
using System.Text.Json;

namespace RMS.Services.Services.CacheServices
{
    public class CacheService : ICacheService
    {
        private readonly ICacheRepository _cacheRepository;

        public CacheService(ICacheRepository cacheRepository)
        {
            _cacheRepository = cacheRepository;
        }
        public async Task<string?> GetAsync(string cacheKey)
        {
            return await _cacheRepository.GetAsync(cacheKey);
        }

        public async Task SetAsync(string cacheKey, object cacheValue, TimeSpan timeToLive)
        {
            var value = JsonSerializer.Serialize(cacheValue, new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
            await _cacheRepository.SetAsync(cacheKey, value, timeToLive);
        }
    }
}
