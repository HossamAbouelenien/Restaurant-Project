namespace RMS.Shared.DTOs.IdentityDTOs
{
    public class UserDTO
    {
        public string Id { get; set; }

        public string Email { get; set; } = default!;

        public string Name { get; set; } = default!;

        public string Role { get; set; } = default!;
        public string? ConfirmationToken { get; set; }

        public int? BranchId { get; set; }
        public string? BranchName { get; set; }
    }
}