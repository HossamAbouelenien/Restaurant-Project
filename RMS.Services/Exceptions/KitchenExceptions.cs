using RMS.Services.Exceptions.Base;
using RMS.Shared.SharedResources;

namespace RMS.Services.Exceptions
{
    public sealed class KitchenTicketNotFoundException(int id)
        : NotFoundException(SharedResourcesKeys.KitchenTicketNotFound, id)
    { }
}