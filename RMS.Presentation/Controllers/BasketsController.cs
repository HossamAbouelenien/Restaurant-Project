using Microsoft.AspNetCore.Mvc;
using RMS.ServicesAbstraction.IServices.IBasketServices;
using RMS.Shared.DTOs.BasketDTOs;

namespace RMS.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BasketsController : ControllerBase
    {
        private readonly IBasketService _basketService;

        public BasketsController(IBasketService basketService)
        {
            _basketService = basketService;
        }

        [HttpGet]
        public async Task<ActionResult<BasketDTO>> GetBasket([FromQuery] string id)
        {
            var basket = await _basketService.GetBasketAsync(id);
            return Ok(basket);
        }

        [HttpPost]
        public async Task<ActionResult<BasketDTO>> CreateOrUpdateBasket([FromBody] BasketDTO basket)
        {
            var result = await _basketService.CreateOrUpdateBasketAsync(basket);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteBasket([FromRoute] string id)
        {
            var result = await _basketService.DeleteBasketAsync(id);
            return Ok(result);
        }
    }
}