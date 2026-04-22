using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using RMS.Domain.Entities;
using RMS.Services.PaymobServices;
using RMS.ServicesAbstraction.IPaymentServices;

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

        [HttpPost("pay/{orderId:int}")]
        public async Task<IActionResult> Pay(int orderId)
        {
            var userId = User.FindFirst("sub")?.Value;

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

            // 🔐 HMAC validation
            var isValid = PaymobHmacHelper.ValidateHmac(dto, secret);

            if (!isValid)
                return Unauthorized("Invalid HMAC");

            // 💡 مهم: خليه fire-and-forget logic داخل service
            await _payment.HandleWebhookAsync(dto);

            return Ok();
        }
    }
}