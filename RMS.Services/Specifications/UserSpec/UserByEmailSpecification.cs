using RMS.Domain.Entities;
using RMS.Services.Specifications;

public class UserByEmailSpecification : BaseSpecifications<User>
{
    public UserByEmailSpecification(string email)
        : base(u => u.Email == email)
    {
        AddInclude(u => u.Branch);
    }
}