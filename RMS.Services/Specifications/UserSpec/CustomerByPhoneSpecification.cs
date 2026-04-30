using RMS.Domain.Entities;
using RMS.Services.Specifications;
using RMS.Shared.DTOs.Utility;
using RMS.Shared.QueryParams;

public class CustomerByPhoneSpecification : BaseSpecifications<User>
{
    public CustomerByPhoneSpecification(CustomerQueryParams QueryParams)
  : base(u =>
        u.RoleId == SD.Role_Customer &&
        (string.IsNullOrEmpty(QueryParams.phoneNumber) ||
         (u.PhoneNumber != null && u.PhoneNumber.Contains(QueryParams.phoneNumber.Trim())))
    )
    {

        AddInclude(u => u.Addresses);
        ApplyPagination(QueryParams.PageSize, QueryParams.PageIndex);

    }
}