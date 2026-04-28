using System.Text.Json;
using RMS.Domain.Contracts;
using RMS.Domain.Entities.CustomerBasket;
using StackExchange.Redis;

namespace RMS.Persistence.Repositries
{
    public class BasketRepository : IBasketRepository
    {
        private static readonly TimeSpan DefaultTtl = TimeSpan.FromDays(7);
        private readonly IDatabase _database;

        public BasketRepository(IConnectionMultiplexer connection)
        {
            _database = connection.GetDatabase();
        }

        public async Task<CustomerBasket?> CreateOrUpdateBasketAsync(
            CustomerBasket basket,
            TimeSpan timeToLive = default)
        {
            var json = JsonSerializer.Serialize(basket);
            var ttl = timeToLive == default ? DefaultTtl : timeToLive;

            bool saved = await _database.StringSetAsync(basket.Id, json, ttl);

            if (!saved) return null;

            var stored = await _database.StringGetAsync(basket.Id);

            return stored.IsNullOrEmpty
                ? null
                : JsonSerializer.Deserialize<CustomerBasket>(stored!);
        }

        public async Task<CustomerBasket?> GetBasketAsync(string basketId)
        {
            var value = await _database.StringGetAsync(basketId);

            return value.IsNullOrEmpty
                ? null
                : JsonSerializer.Deserialize<CustomerBasket>(value!);
        }

        public async Task<bool> DeleteBasketAsync(string basketId)
        {
            return await _database.KeyDeleteAsync(basketId);
        }
    }
}