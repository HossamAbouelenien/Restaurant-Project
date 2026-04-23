namespace RMS.Shared.Utility
{
    public class PaymobSettings
    {
        public string ApiKey { get; set; }
        public string SecretKey { get; set; }
        public string PublicKey { get; set; }
        public int IntegrationId { get; set; }
        public string HMAC { get; set; }
        public int IframeId { get; set; }

        public PaymobEndpoints EndPoints { get; set; } = new();
    }

    public class PaymobEndpoints
    {
        public string AuthUrl { get; set; }
        public string OrderUrl { get; set; }
        public string PaymentKeyUrl { get; set; }
    }
}
