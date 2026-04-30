namespace RMS.Domain.Entities.CustomerBasket
{
    public class BasketItem
    {
        public int Id { get; set; } = default!; 
        public string Name { get; set; } = default!;
        public string PictureUrl { get; set; } = default!;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
