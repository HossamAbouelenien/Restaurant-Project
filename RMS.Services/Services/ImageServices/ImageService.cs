using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using RMS.ServicesAbstraction.IServices.IImageServices;
using RMS.Shared.DTOs.ImageDTOs;
using System.Net;

namespace RMS.Services.Services.ImageServices
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
