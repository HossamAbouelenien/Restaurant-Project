namespace RMS.Shared.DTOs.ReportsDTOs
{
    public class TopItemsDto
    {
        public int MenuItemId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ArabicName { get; set; } = string.Empty;
        public int OrderCount { get; set; }
    }
}
