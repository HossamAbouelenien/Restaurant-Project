using RMS.Domain.Entities;

namespace RMS.Services.Specifications.MenuItemSpec
{
    internal class MenuItemWithRecipesSpecification : BaseSpecifications<MenuItem>
    {
        public MenuItemWithRecipesSpecification(int menuItemId) : base(m => m.Id  == menuItemId )
        {
            AddInclude(m => m.Recipes);
        }
    }
}
