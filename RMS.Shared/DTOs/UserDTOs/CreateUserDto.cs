namespace RMS.Shared.DTOs.UserDTOs
{
    public class CreateUserDto
    {
        public string Email { get; set; }

        public string Name { get; set; }

        public string RoleId { get; set; }

        public int? BranchId { get; set; }
        
    }
}
