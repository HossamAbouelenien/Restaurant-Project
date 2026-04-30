using RMS.Domain.Entities;

namespace RMS.Services.Specifications.CategorySpec
{
    public class CategoryWithIncludingMenuItemsByIDForDelete : BaseSpecifications<Category>
    {
        public CategoryWithIncludingMenuItemsByIDForDelete(int id ) : base(c => c.Id == id && !c.IsDeleted)
        {
            
            AddInclude(c => c.MenuItems);

        }

    }
}
