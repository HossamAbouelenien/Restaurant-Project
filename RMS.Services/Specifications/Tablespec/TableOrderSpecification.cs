using RMS.Domain.Entities;
using RMS.Shared.QueryParams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Services.Specifications.Tablespec
{
    public class TableOrderSpecification : BaseSpecifications<TableOrder>
    {
        public TableOrderSpecification(TableOrderQueryParams queryParams)
            : base(to =>
                (!queryParams.TableId.HasValue || to.TableId == queryParams.TableId.Value) &&
                (!queryParams.Active.HasValue ||
                    (queryParams.Active.Value ? to.CompletedAt == null : to.CompletedAt != null)) &&
                !to.IsDeleted
            )
        {
            AddInclude(to => to.Table!);
            AddInclude(to => to.Order!);
        }
    }
}
