using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RMS.Domain.Entities;

namespace RMS.Services.Specifications.CategorySpec
{
    internal class CategoryWithIncludingMenuItemOrderedByName : BaseSpecifications<Category>
    {
        public CategoryWithIncludingMenuItemOrderedByName() : base(null)
        {
         
            AddInclude(c => c.MenuItems);
            AddOrderBy(c => c.Name);

        }
    }
}
