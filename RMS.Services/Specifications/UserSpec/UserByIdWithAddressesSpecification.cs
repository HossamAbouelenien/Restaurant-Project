using RMS.Domain.Entities;

namespace RMS.Services.Specifications.UserSpec
{
    public class UserByIdWithAddressesSpecification : BaseSpecifications<User>
    {
        public UserByIdWithAddressesSpecification(string userId)
            : base(u => u.Id == userId)
        {
            AddInclude(u => u.Addresses);
        }
    }
}
