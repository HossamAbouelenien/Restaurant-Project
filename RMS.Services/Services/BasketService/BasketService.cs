using AutoMapper;
using RMS.Domain.Contracts;
using RMS.Domain.Entities.CustomerBasket;
using RMS.Services.Exceptions;
using RMS.ServicesAbstraction;
using RMS.Shared.DTOs.BasketDTOs;

namespace RMS.Services.Services.BasketService
{
    public class BasketService : IBasketService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;

        public BasketService(IBasketRepository basketRepository, IMapper mapper)
        {
            _basketRepository = basketRepository;
            _mapper = mapper;
        }

        public async Task<BasketDTO> GetBasketAsync(string basketId)
        {
            var basket = await _basketRepository.GetBasketAsync(basketId);

            if (basket is null)
                throw new BasketNotFoundException(basketId);

            return _mapper.Map<BasketDTO>(basket);
        }

        public async Task<BasketDTO> CreateOrUpdateBasketAsync(BasketDTO basketDto)
        {
            var customerBasket = _mapper.Map<CustomerBasket>(basketDto);

            var result = await _basketRepository.CreateOrUpdateBasketAsync(customerBasket);

            if (result is null)
                throw new BasketOperationFailedException(basketDto.Id);

            return _mapper.Map<BasketDTO>(result);
        }

        public async Task<bool> DeleteBasketAsync(string basketId)
        {
            var exists = await _basketRepository.GetBasketAsync(basketId);

            if (exists is null) return false;

            return await _basketRepository.DeleteBasketAsync(basketId);
        }
    }
}