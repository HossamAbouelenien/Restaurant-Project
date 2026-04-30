using RMS.Domain.Entities;

namespace RMS.Services.Specifications.IngredientSpec
{
    public class IngredientSpecification  : BaseSpecifications<Ingredient>
    {
        public IngredientSpecification(int pageIndex, int pageSize)
            : base(i => !i.IsDeleted)
        {
            ApplyPagination(pageSize, pageIndex);
            AddOrderBy(i => i.Name); // optional sorting
        }
    }
}
