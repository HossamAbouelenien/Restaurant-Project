namespace RMS.Domain.Enums
{
    public enum OrderStatus : byte
    {
        Received,
        Preparing,
        Ready,
        Delivered,
        Cancelled,
        AwaitingPayment
    }
}
