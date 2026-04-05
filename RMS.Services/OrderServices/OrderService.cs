using AutoMapper;
using RMS.Domain.Contracts;
using RMS.Domain.Entities;
using RMS.Services.Specifications.OrderSpec;
using RMS.ServicesAbstraction;
using RMS.Shared.DTOs.OrderDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Services.OrderServices
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrderService(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<OrderDTO> CreateOrderAsync(CreateOrderDTO orderDto)
        {
            var Repo = _unitOfWork.GetRepository<Order>();
            var ItemRepo = _unitOfWork.GetRepository<MenuItem>();
            var orderItems = new List<OrderItemDTO>();
            // Validate branch and customer existence
            var branchExists = await _unitOfWork.GetRepository<Branch>().GetByIdAsync(orderDto.BranchId) != null;
            var UserSpec = new UserSpecification(orderDto.CustomerId!);
            var customerExists = await _unitOfWork.GetRepository<User>().GetByIdAsync(UserSpec) != null;
            if (!branchExists) throw new Exception("Branch not found");
            if (!customerExists) throw new Exception("Customer not found");
            foreach (var item in orderDto.Items)
            {
                var menuItem = await ItemRepo.GetByIdAsync(item.MenuItemId) ?? throw new Exception("Menu item not found");
                orderItems.Add(new OrderItemDTO { MenuItemId = menuItem.Id, Quantity = item.Quantity, UnitPrice = menuItem.Price, Notes=item.Notes}); 
            }
            orderDto.Items = orderItems;
            var order = _mapper.Map<Order>(orderDto);                 
            order.TotalAmount = orderItems.Sum(i => i.Quantity * i.UnitPrice);
            await Repo.AddAsync(order);
            var result = await _unitOfWork.SaveChangesAsync();
            if (result > 0)
            {
                var Spec = new OrderWithBranchAndCustomerAndOrderItemsSpecification(order.Id);
                order = await Repo.GetByIdAsync(Spec) ?? throw new Exception("Failed to retrieve created order");
                return _mapper.Map<OrderDTO>(order);               
            }
            throw new Exception("Failed to create order");
        }
    }
}
