using RMS.Domain.Entities;

namespace RMS.Services.Specifications.IdentitySpec
{
    public class UserProviderByProviderSpec : BaseSpecifications<UserProvider>
    {
        public UserProviderByProviderSpec(string provider, string providerId)
            : base(up => up.Provider == provider && up.ProviderId == providerId)
        {
            AddInclude(up => up.User);
        }
    }
}
