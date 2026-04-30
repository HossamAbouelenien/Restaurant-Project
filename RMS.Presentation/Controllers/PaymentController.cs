using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using RMS.Services.Services.PaymobServices;
using RMS.ServicesAbstraction.IServices.IPaymentServices;
using RMS.Shared.DTOs.Utility;
using RMS.Shared.QueryParams;
using System.Security.Claims;

namespace RMS.Presentation.Controllers
{
    [ApiController]
    [Route("api/payment")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _payment;
        private readonly IConfiguration _config;

        public PaymentController(IPaymentService payment, IConfiguration config)
        {
            _payment = payment;
            _config = config;
        }

        [Authorize]
        [HttpPost("pay/{orderId:int}")]
        public async Task<IActionResult> Pay(int orderId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized("User not authenticated");

            var url = await _payment.PayOrderAsync(orderId, userId);

            return Ok(new { iframeUrl = url });
        }

        [HttpPost("webhook")]
        public async Task<IActionResult> Webhook([FromBody] PaymobWebhookDto dto)
        {
            if (dto == null || dto.Obj == null)
                return BadRequest("Invalid payload");

            var secret = _config["Paymob:SecretKey"];

            if (string.IsNullOrWhiteSpace(secret))
                return StatusCode(500, "Missing Paymob secret");

           
            var isValid = PaymobHmacHelper.ValidateHmac(dto, secret);

            if (!isValid)
                return Unauthorized("Invalid HMAC");

            
            await _payment.HandleWebhookAsync(dto);

            return Ok();
        }

        [Authorize]
        [HttpPost("confirm-cash/{orderId}")]
        public async Task<IActionResult> ConfirmCashPayment(int orderId, [FromBody] decimal dto)
        {
            await _payment.ConfirmCashPaymentAsync(orderId , dto);
            return Ok();
        }

        [Authorize(Roles = SD.Role_Admin + "" + SD.Role_Cashier)]
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaymentQueryParams queryParams)
        {
            var result = await _payment.GetAllAsync(queryParams);
            return Ok(result);
        }

        [Authorize(Roles = SD.Role_Admin + "" + SD.Role_Cashier)]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllWithoutPagination()
        {
            var result = await _payment.GetAllWithoutPaginationAsync();
            return Ok(result);
        }

    }
}