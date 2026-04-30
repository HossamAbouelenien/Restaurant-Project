using RMS.Domain.Entities;

namespace RMS.Services.Specifications.MenuItemSpec
{
    internal class PopularMenuItemsSpecification : BaseSpecifications<MenuItem>
    {
        public PopularMenuItemsSpecification(int limit, int? branchId)
            : base(m =>
            m.IsAvailable &&
            (
                !branchId.HasValue ||
                m.Recipes.All(r =>
                    r.Ingredient != null &&
                    r.Ingredient.BranchStocks.Any(bs =>
                        bs.BranchId == branchId &&
                        bs.QuantityAvailable >= r.QuantityRequired
                    )
                )
            )
        )
        {
            AddInclude(m => m.Category!);

            AddOrderByDescending(m =>
                m.OrderItems.Sum(oi => (int?)oi.Quantity) ?? 0
            );

            ApplyPagination(limit, 1);
        }
    }
}

