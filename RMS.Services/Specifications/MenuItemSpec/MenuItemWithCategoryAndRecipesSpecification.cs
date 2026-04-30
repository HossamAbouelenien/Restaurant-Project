using RMS.Domain.Entities;

namespace RMS.Services.Specifications.MenuItemSpec
{
    public class MenuItemWithCategoryAndRecipesSpecification :BaseSpecifications<MenuItem>
    {
        public MenuItemWithCategoryAndRecipesSpecification(int id) : base(m => m.Id == id)
        {
            AddInclude(m => m.Category!);
            // Include Recipes then ThenInclude Ingredient using dot notation
            AddInclude("Recipes.Ingredient");
            
        }
    }
}
