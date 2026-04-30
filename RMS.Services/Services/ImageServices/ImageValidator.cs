using Microsoft.AspNetCore.Http;
using RMS.ServicesAbstraction.IImageServices;

namespace RMS.Services.Services.ImageServices
{
    public class ImageValidator : IImageValidator
    {
        private static readonly string[] AllowedTypes =
        {
        "image/jpeg",
        "image/png",
        "image/webp"
    };

        private const long MaxSize = 2 * 1024 * 1024;

        public bool Validate(IFormFile file, out string? error)
        {
            if (file == null || file.Length == 0)
            {
                error = "Empty file";
                return false;
            }

            if (!AllowedTypes.Contains(file.ContentType))
            {
                error = "Invalid file type (jpg, png, webp only)";
                return false;
            }

            if (file.Length > MaxSize)
            {
                error = "Max size is 2MB";
                return false;
            }

            error = null;
            return true;
        }
    }
}
