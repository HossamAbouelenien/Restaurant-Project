using RMS.Domain.Entities;

namespace RMS.Services.Specifications.MenuItemSpec
{
    public class IngredientByIdsSpecification : BaseSpecifications<Ingredient>
    {
        public IngredientByIdsSpecification(HashSet<int> ingredientIds): base(i => ingredientIds.Contains(i.Id))
        {
        }
    }
}
