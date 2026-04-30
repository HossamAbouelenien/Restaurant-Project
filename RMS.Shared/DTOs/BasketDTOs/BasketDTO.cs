namespace RMS.Shared.DTOs.BasketDTOs
{
    public record BasketDTO(string Id, ICollection<BasketItemDto> Items);
}
