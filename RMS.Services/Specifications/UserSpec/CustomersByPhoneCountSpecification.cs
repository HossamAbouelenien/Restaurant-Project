using RMS.Domain.Entities;
using RMS.Services.Specifications;
using RMS.Shared.DTOs.Utility;
using RMS.Shared.QueryParams;

public class CustomersByPhoneCountSpecification : BaseSpecifications<User>
{
    public CustomersByPhoneCountSpecification(CustomerQueryParams queryParams)
        : base(u =>
            u.RoleId == SD.Role_Customer && 
            (string.IsNullOrEmpty(queryParams.phoneNumber) ||
            u.PhoneNumber!.Contains(queryParams.phoneNumber))
        
        )
    {
    }
}
