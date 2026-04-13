using RMS.Domain.Entities;
using RMS.Services.Specifications;
using RMS.Shared.DTOs.Utility;
using RMS.Shared.QueryParams;
using System.ComponentModel;
using System.Linq.Expressions;

public class CustomerByPhoneSpecification : BaseSpecifications<User>
{
    public CustomerByPhoneSpecification(CustomerQueryParams QueryParams)
  : base(u =>
            u.RoleId == SD.Role_Customer && // ✅ فلتر الـ Role
            (string.IsNullOrEmpty(QueryParams.phoneNumber) ||
            u.PhoneNumber!.Contains(QueryParams.phoneNumber))
)
    {

        AddInclude(u => u.Addresses);
        ApplyPagination(QueryParams.PageIndex, QueryParams.PageSize);

    }
}