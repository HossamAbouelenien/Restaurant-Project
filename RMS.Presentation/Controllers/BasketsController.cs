using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RMS.ServicesAbstraction.IServices.IBasketServices;
using RMS.Shared.DTOs.BasketDTOs;

namespace RMS.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BasketsController : ControllerBase
    {
        private readonly IBasketService _basketService;
        private readonly ILogger<BasketsController> _logger;

        public BasketsController(IBasketService basketService, ILogger<BasketsController> logger)
        {
            _basketService = basketService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<BasketDTO>> GetBasket([FromQuery] string id)
        {
            _logger.LogInformation("GetBasket request started");
            var basket = await _basketService.GetBasketAsync(id);
            return Ok(basket);
        }

        [HttpPost]
        public async Task<ActionResult<BasketDTO>> CreateOrUpdateBasket([FromBody] BasketDTO basket)
        {
            _logger.LogInformation("CreateOrUpdateBasket request started");
            var result = await _basketService.CreateOrUpdateBasketAsync(basket);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteBasket([FromRoute] string id)
        {
            _logger.LogInformation("DeleteBasket request started");
            var result = await _basketService.DeleteBasketAsync(id);
            return Ok(result);
        }
    }
}