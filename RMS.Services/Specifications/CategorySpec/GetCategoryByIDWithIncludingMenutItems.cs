using RMS.Domain.Entities;

namespace RMS.Services.Specifications.CategorySpec
{
    public class GetCategoryByIDWithIncludingMenutItems : BaseSpecifications<Category>
    {
        public GetCategoryByIDWithIncludingMenutItems(int id):base(c => c.Id == id && !c.IsDeleted)
        {
            AddInclude(c => c.MenuItems);

        }
    }
}
