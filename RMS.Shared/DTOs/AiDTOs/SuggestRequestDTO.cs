namespace RMS.Shared.DTOs.AiDTOs
{
    public class SuggestRequestDTO
    {
        public List<string> Ingredients { get; set; } = new();
        public double MinScore { get; set; } = 0;
    }
}
