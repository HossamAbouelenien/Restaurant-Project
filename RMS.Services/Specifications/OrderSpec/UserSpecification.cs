using RMS.Domain.Entities;

namespace RMS.Services.Specifications.OrderSpec
{
    internal class UserSpecification : BaseSpecifications<User>
    {
            public UserSpecification(string userId) : base(u => u.Id == userId)
            {}
    }
}
