namespace RMS.Shared.DTOs.OrderDTOs
{
    public class OrderAddressDTO
    {
        public int BuildingNumber { get; set; }
        public string Street { get; set; } = default!;
        public string City { get; set; } = default!;
        public string? Note { get; set; }
        public string? SpecialMark { get; set; }
    }
}
