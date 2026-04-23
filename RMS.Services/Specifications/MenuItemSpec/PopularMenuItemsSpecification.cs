using RMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Services.Specifications.MenuItemSpec
{
    internal class PopularMenuItemsSpecification : BaseSpecifications<MenuItem>
    {
        public PopularMenuItemsSpecification(int limit)
            : base(m => m.IsAvailable)
        {
            AddInclude(m => m.Category!);

            AddOrderByDescending(m =>
                m.OrderItems.Sum(oi => (int?)oi.Quantity) ?? 0
            );

            ApplyPagination(limit, 1);
        }
    }
}

