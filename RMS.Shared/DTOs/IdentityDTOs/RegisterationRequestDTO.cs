using System.ComponentModel.DataAnnotations;

namespace RMS.Shared.DTOs.IdentityDTOs
{
    public class RegisterationRequestDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        public string Password { get; set; }

        [MaxLength(50)]
        public string Role { get; set; } = "Customer";
    }
}