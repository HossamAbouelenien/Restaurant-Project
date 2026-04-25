using Microsoft.AspNetCore.Http;
using RMS.Shared.DTOs.ImageDTOs;

namespace RMS.ServicesAbstraction.IImageServices
{
    public interface IImageService
    {

        Task<ImageAsset?> UploadImageAsync(IFormFile? file);
        Task<bool> DeleteImageAsync(string? publicId);
        Task<ImageAsset?> ReplaceImageAsync(IFormFile? file, string? oldPublicId);
    }
}
