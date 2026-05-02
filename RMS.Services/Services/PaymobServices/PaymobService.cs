using Microsoft.Extensions.Options;
using RMS.ServicesAbstraction.IServices.IPaymobServices;
using RMS.Shared.Utility;
using System.Net.Http.Json;
using System.Text.Json;

public class PaymobService : IPaymobService
{
    private readonly HttpClient _http;
    private readonly PaymobSettings _settings;

    public PaymobService(HttpClient http, IOptions<PaymobSettings> options)
    {
        _http = http;
        _settings = options.Value;
    }

    public async Task<string> GetPaymentKeyAsync(decimal amount, int orderId)
    {
        // 1. AUTH
        var authResponse = await _http.PostAsJsonAsync(
            _settings.EndPoints.AuthUrl,
            new { api_key = _settings.ApiKey });

        var authJson = await authResponse.Content.ReadFromJsonAsync<JsonElement>();
        var authToken = authJson.GetProperty("token").GetString();

        // 2. ORDER
        var orderResponse = await _http.PostAsJsonAsync(
            _settings.EndPoints.OrderUrl,
            new
            {
                auth_token = authToken,
                amount_cents = (int)(amount * 100),
                currency = "EGP",
                delivery_needed = false,
                merchant_order_id = $"{orderId}_{Guid.NewGuid():N}",
                items = new object[] { }
            });

        var orderJson = await orderResponse.Content.ReadFromJsonAsync<JsonElement>();
        var paymobOrderId = orderJson.GetProperty("id").GetInt32();

        // 3. PAYMENT KEY
        var paymentResponse = await _http.PostAsJsonAsync(
            _settings.EndPoints.PaymentKeyUrl,
            new
            {
                auth_token = authToken,
                amount_cents = (int)(amount * 100),
                expiration = 3600,
                order_id = paymobOrderId,
                currency = "EGP",
                integration_id = _settings.IntegrationId,

                billing_data = new
                {
                    email = "test@test.com",
                    first_name = "Test",
                    last_name = "User",
                    phone_number = "01000000000",
                    country = "EG",
                    city = "Cairo",
                    street = "N/A",
                    building = "N/A",
                    floor = "N/A",
                    apartment = "N/A"
                }
            });

        var paymentJson = await paymentResponse.Content.ReadFromJsonAsync<JsonElement>();
        return paymentJson.GetProperty("token").GetString()!;
    }

    public string BuildIframeUrl(string paymentToken)
    {
        return $"https://accept.paymob.com/api/acceptance/iframes/{_settings.IframeId}?payment_token={paymentToken}";
    }
}