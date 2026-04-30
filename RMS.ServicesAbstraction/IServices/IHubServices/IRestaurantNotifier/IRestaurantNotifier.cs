namespace RMS.ServicesAbstraction.IServices.IHubServices.IRestaurantNotifier
{
    public interface IRestaurantNotifier
    {
        Task SendAsync(string eventName, object data, params string[] groups);

    }
}
