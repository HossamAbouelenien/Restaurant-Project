using RMS.Domain.Entities;
using RMS.Domain.Enums;
using RMS.Shared.QueryParams;

namespace RMS.Services.Specifications.KitchenTicketSpec
{
    public class ActiveStationsForBranchWithPendingTicketCountSpecification : BaseSpecifications<KitchenTicket>
    {
        public ActiveStationsForBranchWithPendingTicketCountSpecification(int branchId)
       : base(b =>b.Status == TicketStatus.Pending && b.Order!.BranchId == branchId)                   
        {
            
        }
    }
}
