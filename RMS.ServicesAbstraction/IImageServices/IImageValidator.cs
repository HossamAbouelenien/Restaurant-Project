using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.ServicesAbstraction.IImageServices
{
    public interface IImageValidator
    {
        bool Validate(IFormFile file, out string? error);
    }
}
