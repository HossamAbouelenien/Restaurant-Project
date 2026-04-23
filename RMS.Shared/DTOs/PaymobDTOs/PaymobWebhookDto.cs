public class PaymobWebhookDto
{
    public PaymobWebhookObj Obj { get; set; } = new();
    public string Hmac { get; set; } = string.Empty;
}