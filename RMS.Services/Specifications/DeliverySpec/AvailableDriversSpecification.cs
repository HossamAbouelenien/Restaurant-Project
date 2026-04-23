using RMS.Domain.Entities;
using RMS.Domain.Enums;
using RMS.Services.Specifications;

public class AvailableDriversSpecification : BaseSpecifications<User>
{
    public AvailableDriversSpecification(AvailableDriversQueryParams query)
        : base(u =>
            u.RoleId == "Driver" &&
            (query.BranchId == null || u.BranchId == query.BranchId) &&       
            (string.IsNullOrEmpty(query.Search) ||
             u.Name.Contains(query.Search) ||
             u.PhoneNumber.Contains(query.Search))
            //!u.Deliveries.Any(d => d.DeliveryStatus != DeliveryStatus.Delivered)
        )
    {
       
        AddInclude(u => u.Branch!);
        ApplyPagination(query.PageSize, query.PageIndex);
    }
}