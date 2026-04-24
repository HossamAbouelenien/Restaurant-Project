using Microsoft.AspNetCore.Http;
using RMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.ServicesAbstraction
{
    public interface IImageService
    {
        //Task<string?> UploadImageAsync(IFormFile? file);
        //Task DeleteImageAsync(string? imagePath);
        //Task<string?> ReplaceImageAsync(IFormFile? file, string? oldImagePath);

        Task<ImageAsset?> UploadImageAsync(IFormFile? file);
        Task<bool> DeleteImageAsync(string? publicId);
        Task<ImageAsset?> ReplaceImageAsync(IFormFile? file, string? oldPublicId);
    }
}
