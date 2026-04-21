using RMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
