using RMS.Domain.Entities;

namespace RMS.Services.Specifications.OrderSpec
{
    internal class OrderWithTableOrderAndBranchAndCustomerAndOrderItemsSpecification : BaseSpecifications<Order>
    {
        public OrderWithTableOrderAndBranchAndCustomerAndOrderItemsSpecification(int orderId) 
            : base(o => o.Id == orderId)
        {
            AddInclude(o => o.User!);
            AddInclude(o => o.Branch!);
            AddInclude(o => o.OrderItems);
            AddInclude(o => o.TableOrder!);
            AddInclude(o => o.Delivery!);
            AddInclude(o => o.Payment!);
            AddInclude("OrderItems.MenuItem");
            //AddInclude("Customer.Role");


        }

    }
}
