namespace RMS.Shared.DTOs.TableDTOs
{
    public class TableDetailsDTO
    {
        public int Id { get; set; }
        public int BranchId { get; set; }
        public string TableNumber { get; set; } = default!;
        public bool IsOccupied { get; set; } = false;
        public int Capacity { get; set; }
    }
}
