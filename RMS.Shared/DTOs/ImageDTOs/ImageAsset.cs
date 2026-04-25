namespace RMS.Shared.DTOs.ImageDTOs
{
    public class ImageAsset
    {
        public string Id { get; set; }
        public string Url { get; set; }
        public string PublicId { get; set; }

        public int? Width { get; set; }
        public int? Height { get; set; }
        public string? Format { get; set; }
    }
}
