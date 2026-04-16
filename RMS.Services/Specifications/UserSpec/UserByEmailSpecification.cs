using RMS.Domain.Entities;
using RMS.Services.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class UserByEmailSpecification : BaseSpecifications<User>
{
    public UserByEmailSpecification(string email)
        : base(u => u.Email == email)
    {
        AddInclude(u => u.Branch);
    }
}