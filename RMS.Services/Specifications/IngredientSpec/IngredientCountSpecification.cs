using RMS.Domain.Entities;

namespace RMS.Services.Specifications.IngredientSpec
{
    public class IngredientCountSpecification : BaseSpecifications<Ingredient>
    {
        public IngredientCountSpecification()
            : base(i => !i.IsDeleted)
        {
        }
    }
}
