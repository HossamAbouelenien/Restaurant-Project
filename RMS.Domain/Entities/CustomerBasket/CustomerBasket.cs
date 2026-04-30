namespace RMS.Domain.Entities.CustomerBasket
{
    public class CustomerBasket
    {
        public string Id { get; set; } = default!; // GUID : Created from client side ( FrontEnd )
        public ICollection<BasketItem> Items { get; set; } = [];
    }
}
