using RMS.Services.Exceptions.Base;
using RMS.Shared.SharedResources;

namespace RMS.Services.Exceptions
{
    public sealed class NotificationNotFoundException(int id)
         : NotFoundException(SharedResourcesKeys.NotificationNotFound, id)
    { }

}