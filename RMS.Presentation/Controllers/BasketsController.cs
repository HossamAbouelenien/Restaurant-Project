using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RMS.ServicesAbstraction;
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
            this._basketService = basketService;
        }


        [HttpGet]
        public async Task<ActionResult<BasketDTO>> GetBasket(string id)
        {
            var basket = await _basketService.GetBasketAsync(id);
            return Ok(basket);
        }

        [HttpPost]
        public async Task<ActionResult<BasketDTO>> CreateOrUpdateBasket(BasketDTO basket)
        {
            var CreatedOrUpdatedBasket = await _basketService.CreateOrUpdateBasketAsync(basket);
            return Ok(CreatedOrUpdatedBasket);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteBasket(string id)
        {
            var result = await _basketService.DeleteBasketAsync(id);
            return Ok(result);
        }
    }
}
