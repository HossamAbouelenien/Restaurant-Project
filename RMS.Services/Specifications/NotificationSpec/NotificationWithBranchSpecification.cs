using RMS.Domain.Entities;
using RMS.Shared.QueryParams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Services.Specifications.NotificationSpec
{
    internal class NotificationWithBranchSpecification : BaseSpecifications<Notification>
    {
        public NotificationWithBranchSpecification(NotificationQueryParams queryParams)
            :base(n => (queryParams.UserId == null || n.UserId == queryParams.UserId) &&
                                       (queryParams.Role == null || n.Role == queryParams.Role) &&
                                         (!queryParams.BranchId.HasValue || n.BranchId == queryParams.BranchId))
        {
            AddInclude(n => n.Branch!);
        }
    }
}
