public class AvailableDriverDto
{
    public string DriverId { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public int? BranchId { get; set; }
    public string? BranchName { get; set; }
}
