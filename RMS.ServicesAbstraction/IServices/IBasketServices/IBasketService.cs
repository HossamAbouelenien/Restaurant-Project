using RMS.Shared.DTOs.BasketDTOs;

namespace RMS.ServicesAbstraction.IServices.IBasketServices
{
    public interface IBasketService
    {

        Task<BasketDTO> GetBasketAsync(string basketId);
        Task<BasketDTO> CreateOrUpdateBasketAsync(BasketDTO basket);
        Task<bool> DeleteBasketAsync(string basketId);


    }
}
