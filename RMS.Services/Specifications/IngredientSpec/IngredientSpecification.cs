using RMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
