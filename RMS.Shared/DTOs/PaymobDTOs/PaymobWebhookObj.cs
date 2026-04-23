public class PaymobWebhookObj
{
    public bool Success { get; set; }
    public string Order { get; set; } = string.Empty;
    public string AmountCents { get; set; } = string.Empty;
    public string CreatedAt { get; set; } = string.Empty;
    public string Currency { get; set; } = string.Empty;
    public string ErrorOccured { get; set; } = string.Empty;
    public string HasParentTransaction { get; set; } = string.Empty;
    public string Id { get; set; } = string.Empty;
    public string IntegrationId { get; set; } = string.Empty;
    public string Is3DSecure { get; set; } = string.Empty;
    public string IsAuth { get; set; } = string.Empty;
    public string IsCapture { get; set; } = string.Empty;
    public string IsRefunded { get; set; } = string.Empty;
    public string IsStandalonePayment { get; set; } = string.Empty;
    public string IsVoided { get; set; } = string.Empty;
    public string Owner { get; set; } = string.Empty;
    public string Pending { get; set; } = string.Empty;
    public string SourceDataPan { get; set; } = string.Empty;
    public string SourceDataSubType { get; set; } = string.Empty;
    public string SourceDataType { get; set; } = string.Empty;
}