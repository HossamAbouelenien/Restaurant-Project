namespace RMS.Shared.DTOs.UserDTOs
{
    public class GetUserDTO
    {
        public string Id { get; set; }

        //public string UserName { get; set; }

        public string Email { get; set; }

        public string Name { get; set; }

        public string RoleId { get; set; }

        public int? BranchId { get; set; }

        public string? BranchName { get; set; }
        public string? BranchArabicName { get; set; }

        public bool IsDeleted { get; set; }

        //public DateTime CreatedAt { get; set; }
    }
}
