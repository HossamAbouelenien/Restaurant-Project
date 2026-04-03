using System.ComponentModel.DataAnnotations;

namespace RMS.Shared.DTOs.IdentityDTOs
{
    public class LoginRequestDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}