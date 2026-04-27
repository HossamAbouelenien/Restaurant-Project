using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using RMS.Domain.Contracts;
using RMS.Domain.Entities.CustomerBasket;
using RMS.Services.Exceptions;
using RMS.ServicesAbstraction;
using RMS.Shared.DTOs.BasketDTOs;

namespace RMS.Services.BasketService
{
    public class BasketService : IBasketService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;
        public BasketService(IBasketRepository basketRepository, IMapper mapper)
        {
            this._basketRepository = basketRepository;
            this._mapper = mapper;
        }

        public async Task<BasketDTO> CreateOrUpdateBasketAsync(BasketDTO basket)
        {
            var customerBasket = _mapper.Map<CustomerBasket>(basket);

            var CreatedOrUpdatedBasket = await _basketRepository.CreateOrUpdateBasketAsync(customerBasket);

            if(CreatedOrUpdatedBasket is null)
            {
                throw new BasketOperationFailedException(basket.Id);
            }

            return _mapper.Map<BasketDTO>(CreatedOrUpdatedBasket);
        }

        public async Task<bool> DeleteBasketAsync(string basketId)
        {

            var basket = await _basketRepository.GetBasketAsync(basketId);

            if(basket is null)
            {
                throw new BasketNotFoundException(basketId);
            }
            return await _basketRepository.DeleteBasketAsync(basketId);

        }


        public async Task<BasketDTO> GetBasketAsync(string basketId)
        {

            var Basket = await _basketRepository.GetBasketAsync(basketId);

            if(Basket is null)
            {
                throw new BasketNotFoundException(basketId);
            }

            return _mapper.Map<BasketDTO>(Basket!);

        }

    }

}
