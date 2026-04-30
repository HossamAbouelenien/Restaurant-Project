namespace RMS.Shared.Enums
{
   
    [Flags]
    public enum OrderIncludes
    {
        None = 0,
        User = 1,
        Payment = 2,
        OrderItems = 4,
        Delivery = 8,
        TableOrder = 16,
        KitchenTickets = 32,
        UserAddresses = 64
    }
}
