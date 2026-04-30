using RMS.Domain.Entities;

namespace RMS.Services.Specifications.DeliverySpec
{
    public class DeliveryByIdSpecification : BaseSpecifications<Delivery>
    {
        public DeliveryByIdSpecification(int id)
            : base(d => d.Id == id)
        {
            AddInclude(d => d.Order!);
            AddInclude(d => d.Order!.User!);
            AddInclude(d => d.Order!.Branch!);
            AddInclude(d => d.Order!.OrderItems!);
            AddInclude(d => d.Driver!);
            AddInclude(d => d.DeliveryAddress);
            AddInclude("Order.OrderItems.MenuItem");
        }
    }
}
