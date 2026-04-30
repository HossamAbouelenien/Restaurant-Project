namespace RMS.Shared.DTOs.AddressDTOs
{
    public class AddressDto
    {
        public int BuildingNumber { get; set; }

        public string Street { get; set; } = default!;

        public string City { get; set; } = default!;

        public string? Note { get; set; }

        public string? SpecialMark { get; set; }
    }
}
