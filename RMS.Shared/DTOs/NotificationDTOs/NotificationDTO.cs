namespace RMS.Shared.DTOs.NotificationDTOs
{
    public class NotificationDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = default!;
        public string ArabicTitle { get; set; } = default!;
        public string Message { get; set; } = default!;
        public string ArabicMessage { get; set; } = default!;
        public string? BranchName { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
