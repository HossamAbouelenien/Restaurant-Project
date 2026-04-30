using Microsoft.AspNetCore.Http;
using RMS.Shared.DTOs.ImageDTOs;

namespace RMS.ServicesAbstraction.IServices.IImageServices
{
    public interface IImageStorageStrategy
    {
        Task<ImageAsset?> UploadAsync(IFormFile file);
        Task<bool> DeleteAsync(string publicId);
    }
}
