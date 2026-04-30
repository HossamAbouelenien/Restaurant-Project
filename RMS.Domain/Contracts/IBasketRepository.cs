using RMS.Domain.Entities.CustomerBasket;

namespace RMS.Domain.Contracts
{
    public interface IBasketRepository
    {
        Task<CustomerBasket?> GetBasketAsync(string basketId);
        Task<CustomerBasket?> CreateOrUpdateBasketAsync(CustomerBasket basket, TimeSpan timeToLive = default);
        Task<bool> DeleteBasketAsync(string basketId);

    }
}
