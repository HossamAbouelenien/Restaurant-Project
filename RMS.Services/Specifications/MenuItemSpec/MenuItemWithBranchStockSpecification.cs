using RMS.Domain.Entities;

namespace RMS.Services.Specifications.MenuItemSpec
{
    internal class MenuItemWithBranchStockSpecification :BaseSpecifications<MenuItem>
    {
        public MenuItemWithBranchStockSpecification(int id) : base(m => m.Id == id)
        {
            AddInclude("Recipes.Ingredient.BranchStocks");
            AddInclude(m => m.Category!);
        }
    }
}
