using RMS.Domain.Entities;

namespace RMS.Services.Specifications.CategorySpec
{
    internal class CategoryWithIncludingMenuItemOrderedByName : BaseSpecifications<Category>
    {
        public CategoryWithIncludingMenuItemOrderedByName() : base(c => !c.IsDeleted)
        {
         
            AddInclude(c => c.MenuItems);
            AddOrderBy(c => c.Name);

        }
    }
}
