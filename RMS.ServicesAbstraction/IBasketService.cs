using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RMS.Shared.DTOs.BasketDTOs;

namespace RMS.ServicesAbstraction
{
    public interface IBasketService
    {

        Task<BasketDTO> GetBasketAsync(string basketId);
        Task<BasketDTO> CreateOrUpdateBasketAsync(BasketDTO basket);
        Task<bool> DeleteBasketAsync(string basketId);


    }
}
