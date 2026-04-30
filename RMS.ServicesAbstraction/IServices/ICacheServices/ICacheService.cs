namespace RMS.ServicesAbstraction.IServices.ICacheServices
{
    public interface ICacheService
    {
        Task<string?> GetAsync(string cacheKey);
        Task SetAsync(string cacheKey, object cacheValue, TimeSpan timeToLive);
    }
}
