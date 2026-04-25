using RMS.Shared;
using RMS.Shared.DTOs.OrderDTOs;
using RMS.Shared.QueryParams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.ServicesAbstraction
{
    public interface IOrderService
    {
        Task<OrderDTO> CreateOrderAsync(CreateOrderDTO orderDto);
        Task<PaginatedResult<OrderDTO>> GetAllOrdersAsync(OrderQueryParams queryParams);
        Task<OrderDetailsDTO> GetOrderByIdAsync(int id);
        Task<PaginatedResult<OrderDTO>> GetCustomerOrdersHistoryAsync(OrderQueryParams queryParams, string customerId);
        Task<PaginatedResult<MyDeliveryActiveCustomersDTO>> GetCustomerOrdersActiveAsync(OrderQueryParams queryParams, string customerId);
        Task<OrderDTO> UpdateOrderStatusAsync(int orderId, string newStatus);
        Task<AddedItemsDTO> AddItemsToOrderAsync(int orderId, List<CreateOrderItemDTO> items);
        Task<OrderDTO> RemoveItemsFromOrderAsync(int orderId, int itemId);
        Task CancelOrderAsync(int orderId);

    }
}
