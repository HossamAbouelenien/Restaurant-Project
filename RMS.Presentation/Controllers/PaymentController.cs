using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RMS.Services.Services.PaymobServices;
using RMS.ServicesAbstraction.IServices.IPaymentServices;
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
        private readonly ILogger<PaymentController> _logger;

        public PaymentController(IPaymentService payment, IConfiguration config, ILogger<PaymentController> logger)
        {
            _payment = payment;
            _config = config;
            _logger = logger;
        }

        //[Authorize]
        [HttpPost("pay/{orderId:int}")]
        public async Task<IActionResult> Pay(int orderId)
        {
            _logger.LogInformation("Pay request started");
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(userId))
            {
                _logger.LogWarning("User not authenticated: userId is null or empty");
                return Unauthorized("User not authenticated");
            }
                

            var url = await _payment.PayOrderAsync(orderId, userId);

            return Ok(new { iframeUrl = url });
        }

        [HttpPost("webhook")]
        public async Task<IActionResult> Webhook([FromBody] PaymobWebhookDto dto)
        {
            _logger.LogInformation("Payment webhook received");
            if (dto == null || dto.Obj == null)
            {
                _logger.LogWarning("Webhook failed: invalid payload");
                return BadRequest("Invalid payload");
            }
                

            var secret = _config["Paymob:SecretKey"];

            if (string.IsNullOrWhiteSpace(secret))
            {
                _logger.LogError("Webhook failed: Paymob secret key is missing in configuration");
                return StatusCode(500, "Missing Paymob secret");
            }
                

           
            var isValid = PaymobHmacHelper.ValidateHmac(dto, secret);

            if (!isValid)
            {
                _logger.LogWarning("Webhook failed: invalid HMAC");
                return Unauthorized("Invalid HMAC");
            }
                

            
            await _payment.HandleWebhookAsync(dto);

            return Ok();
        }

        //[Authorize]
        [HttpPost("confirm-cash/{orderId}")]
        public async Task<IActionResult> ConfirmCashPayment(int orderId, [FromBody] decimal dto)
        {
            _logger.LogInformation("ConfirmCashPayment request started");
            await _payment.ConfirmCashPaymentAsync(orderId , dto);
            return Ok();
        }

        //[Authorize(Roles = SD.Role_Admin + "" + SD.Role_Cashier)]
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaymentQueryParams queryParams)
        {
            _logger.LogInformation("GetAllPayments request started");
            var result = await _payment.GetAllAsync(queryParams);
            return Ok(result);
        }

        //[Authorize(Roles = SD.Role_Admin + "" + SD.Role_Cashier)]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllWithoutPagination()
        {
            _logger.LogInformation("GetAllPaymentsWithoutPagination request started");
            var result = await _payment.GetAllWithoutPaginationAsync();
            return Ok(result);
        }

    }
}