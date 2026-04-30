namespace RMS.Shared.DTOs.UserDTOs
{
    public class UpdateUserDto
    {
        public string Id { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Name { get; set; } = null!;

        public int? BranchId { get; set; }

        public string? RoleId { get; set; }
    }
}
