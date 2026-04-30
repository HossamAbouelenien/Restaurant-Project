using RMS.Domain.Entities;

namespace RMS.Services.Specifications.UserSpec
{
    public class UserAddressesSpecification : BaseSpecifications<User>
    {
        public UserAddressesSpecification(string userId)
            : base(u => u.Id == userId)
        {
            AddInclude(u => u.Addresses);
        }
    }
}
