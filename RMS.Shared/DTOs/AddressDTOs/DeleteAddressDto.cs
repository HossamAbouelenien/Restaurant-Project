namespace RMS.Shared.DTOs.AddressDTOs
{
    public class DeleteAddressDto
    {
        public int BuildingNumber { get; set; }
        public string Street { get; set; } = default!;
        public string City { get; set; } = default!;
    }
}
