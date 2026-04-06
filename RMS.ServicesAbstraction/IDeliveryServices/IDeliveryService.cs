using RMS.Shared;
using RMS.Shared.DTOs.DeliveryDTOs;
using RMS.Shared.QueryParams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.ServicesAbstraction.IDeliveryServices
{
    public interface IDeliveryService
    {
        Task<PaginatedResult<DeliveryDto>> GetAllDeliveriesAsync(DeliveryQueryParams queryParams);

    }
}
