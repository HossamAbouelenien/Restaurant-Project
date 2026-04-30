namespace RMS.ServicesAbstraction.IServices.IHubServices.INotificationServices
{
    public interface IRealTimeNotifier
    {
        Task NotifyAdmins(object data, string groupName, string eventName);

    }
}
