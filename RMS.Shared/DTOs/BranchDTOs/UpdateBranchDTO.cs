namespace RMS.Shared.DTOs.BranchDTOs
{
   public class UpdateBranchDTO
    {
        public string Name { get; set; } = default!;
        public string ArabicName { get; set; } = default!;
        public string Phone { get; set; } = default!;
        public bool IsActive { get; set; } = true;
        public int BuildingNumber { get; set; }
        public string Street { get; set; } = default!;
        public string City { get; set; } = default!;
        public string? Note { get; set; }
        public string? SpecialMark { get; set; }
    }
}
