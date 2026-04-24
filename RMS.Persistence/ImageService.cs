//using Microsoft.AspNetCore.Hosting;
//using Microsoft.AspNetCore.Http;
//using Microsoft.Extensions.Hosting;
//using RMS.ServicesAbstraction;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace RMS.Persistence
//{
//    public class ImageService : IImageService
//    {
//        private readonly IWebHostEnvironment _env;

//        public ImageService(IWebHostEnvironment env)
//        {
//            _env = env;
//        }
//        #region Helper Methods
//        private string GetImageFolderPath()
//        {
//            return Path.Combine(_env.WebRootPath, "Images");
//        }
//        #endregion

//        public async Task DeleteImageAsync(string? imagePath)
//        {
//            if (string.IsNullOrEmpty(imagePath)) return;

//            var folderPath = GetImageFolderPath();

//            var fullPath = Path.Combine(folderPath, Path.GetFileName(imagePath));

//            if (File.Exists(fullPath))
//                File.Delete(fullPath);
//        }

//        public async Task<string?> ReplaceImageAsync(IFormFile? file, string? oldImagePath)
//        {
//            if (file == null) return oldImagePath;

//            await DeleteImageAsync(oldImagePath);

//            return await UploadImageAsync(file);
//        }

//        public async Task<string?> UploadImageAsync(IFormFile? file)
//        {
//            if (file == null) return null;
//            // Validate Extension
//            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
//            var extension = Path.GetExtension(file.FileName).ToLower();

//            if (!allowedExtensions.Contains(extension))
//                throw new Exception("Invalid file type. Only jpg, jpeg, png, webp allowed.");

//            // Validate Size (2MB)
//            if (file.Length > 2 * 1024 * 1024)
//                throw new Exception("File size must not exceed 2MB.");

//            var folderPath = GetImageFolderPath();

//            if (!Directory.Exists(folderPath))
//                Directory.CreateDirectory(folderPath);

//            var fileName = Guid.NewGuid() + extension;

//            var filePath = Path.Combine(folderPath, fileName);

//            using var stream = new FileStream(filePath, FileMode.Create);
//            await file.CopyToAsync(stream);

//            return $"Images/{fileName}";
//        }
//    }

//}



using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using RMS.Domain.Entities;
using RMS.ServicesAbstraction;
using RMS.ServicesAbstraction.IImageServices;
using System.Net;

namespace RMS.Persistence
{
    public class ImageService : IImageService
    {
        private readonly Cloudinary _cloudinary;
        private readonly ILogger<ImageService> _logger;
        private readonly IImageValidator _validator;

        public ImageService(
            Cloudinary cloudinary,
            ILogger<ImageService> logger,
            IImageValidator validator)
        {
            _cloudinary = cloudinary;
            _logger = logger;
            _validator = validator;
        }

        public async Task<ImageAsset?> UploadImageAsync(IFormFile? file)
        {
            if (file == null)
                return null;

            if (!_validator.Validate(file, out var error))
            {
                _logger.LogWarning("Image validation failed: {Error}", error);
                return null;
            }

            try
            {
                await using var stream = file.OpenReadStream();

                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Folder = "rms-images",
                    UniqueFilename = true,
                    Overwrite = false,
                    Transformation = new Transformation()
                        .Width(800)
                        .Height(800)
                        .Crop("limit")
                        .Quality("auto")
                        .FetchFormat("auto")
                };

                var result = await _cloudinary.UploadAsync(uploadParams);

                if (result.StatusCode != HttpStatusCode.OK || result.Error != null)
                {
                    _logger.LogError("Upload failed: {Error}", result.Error?.Message);
                    return null;
                }

                return new ImageAsset
                {
                    Id = Guid.NewGuid().ToString(),
                    Url = result.SecureUrl.ToString(),
                    PublicId = result.PublicId,
                    Width = result.Width,
                    Height = result.Height,
                    Format = result.Format
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Image upload exception");
                return null;
            }
        }

        public async Task<bool> DeleteImageAsync(string? publicId)
        {
            if (string.IsNullOrWhiteSpace(publicId))
                return false;

            try
            {
                var result = await _cloudinary.DestroyAsync(
                    new DeletionParams(publicId)
                    {
                        ResourceType = ResourceType.Image
                    });

                if (result.Error != null)
                {
                    _logger.LogError("Delete failed: {Error}", result.Error.Message);
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Delete exception");
                return false;
            }
        }

        public async Task<ImageAsset?> ReplaceImageAsync(IFormFile? file, string? oldPublicId)
        {
            if (file == null)
                return null;

            // upload new image
            var newImage = await UploadImageAsync(file);

            // delete old image if exists
            if (!string.IsNullOrWhiteSpace(oldPublicId))
            {
                await DeleteImageAsync(oldPublicId);
            }

            return newImage;
        }
    }
}
