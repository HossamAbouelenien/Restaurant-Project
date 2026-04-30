namespace RMS.Shared.DTOs.OrderDTOs
{
    public class OrderPaymentDTO
    {
        public string PaymentMethod { get; set; } = default!;
        public string PaymentStatus { get; set; } = default!;
        public decimal PaidAmount { get; set; }
        public DateTime? PaidAt { get; set; }
    }
}
