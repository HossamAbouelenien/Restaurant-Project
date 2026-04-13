using RMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
