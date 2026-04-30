using System.ComponentModel.DataAnnotations;

namespace RMS.Shared.DTOs.IdentityDTOs
{
    public class UpdateCurrentUserDTO
    {
        [Required]
        public string Name { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        public string? UserName { get; set; }

        public string? NewPassword { get; set; }

        [Compare("NewPassword")]
        public string? ConfirmPassword { get; set; }


    }
}
