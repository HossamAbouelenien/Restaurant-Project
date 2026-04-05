using RMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Services.Specifications.KitchenTicketSpec
{
    public class KitchenTicketWithOrderSpecification : BaseSpecifications<KitchenTicket>
    {
        public KitchenTicketWithOrderSpecification(int ticketId)
            : base(t => t.Id == ticketId)
        {
            AddInclude(o=>o.Order!);
        }
    }
}
