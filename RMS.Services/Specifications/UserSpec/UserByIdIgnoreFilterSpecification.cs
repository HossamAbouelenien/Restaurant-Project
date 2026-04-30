using RMS.Domain.Entities;

namespace RMS.Services.Specifications.UserSpec
{
    public class UserByIdIgnoreFilterSpecification : BaseSpecifications<User>
    {
        public UserByIdIgnoreFilterSpecification(string id)
             : base(u => u.Id == id)
        {
            IgnoreQueryFilters = true;
        }
    }
}
