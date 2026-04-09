using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.ServicesAbstraction.IEmailServices
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(string to, string subject, string body);


    }
}
