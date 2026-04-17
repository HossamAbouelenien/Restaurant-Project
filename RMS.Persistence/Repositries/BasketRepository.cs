using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using RMS.Domain.Contracts;
using RMS.Domain.Entities.CustomerBasket;
using StackExchange.Redis;

namespace RMS.Persistence.Repositries
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDatabase _database;
        public BasketRepository(IConnectionMultiplexer connection)
        {
            _database = connection.GetDatabase();
        }
        public async Task<CustomerBasket?> CreateOrUpdateBasketAsync(CustomerBasket basket, TimeSpan timeToLive = default)
        {
            var JsonBasket = JsonSerializer.Serialize(basket);

            bool IsCreatedOrUpdated = await _database.StringSetAsync(basket.Id, JsonBasket, (timeToLive == default) ? TimeSpan.FromDays(7) : timeToLive);

            if (IsCreatedOrUpdated)
            {
                var MyBasket = await _database.StringGetAsync(basket.Id);
                return JsonSerializer.Deserialize<CustomerBasket>(MyBasket!);
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> DeleteBasketAsync(string basketId)
        {
            return await _database.KeyDeleteAsync(basketId);
        }

        public async Task<CustomerBasket?> GetBasketAsync(string basketId)
        {
            var Basket = await _database.StringGetAsync(basketId);
            if (Basket.IsNullOrEmpty)
            {
                return null;
            }
            else
            {
                return JsonSerializer.Deserialize<CustomerBasket>(Basket!);
            }
        }
    }
}
