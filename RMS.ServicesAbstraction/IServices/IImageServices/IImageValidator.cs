using Microsoft.AspNetCore.Http;

namespace RMS.ServicesAbstraction.IServices.IImageServices
{
    public interface IImageValidator
    {
        bool Validate(IFormFile file, out string? error);
    }
}
