namespace RMS.Shared.DTOs.IdentityDTOs
{
    public class ResetPasswordDTO
    {
        public string ResetSessionToken { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
