using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using RMS.Domain.Entities;
using RMS.ServicesAbstraction.IImageServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Services.ImageServices
{
    public class CloudinaryStorageStrategy : IImageStorageStrategy
    {
        private readonly Cloudinary _cloudinary;
        private readonly ILogger<CloudinaryStorageStrategy> _logger;
        private readonly IImageValidator _validator;

        public CloudinaryStorageStrategy(
            Cloudinary cloudinary,
            ILogger<CloudinaryStorageStrategy> logger,
            IImageValidator validator)
        {
            _cloudinary = cloudinary;
            _logger = logger;
            _validator = validator;
        }

        public async Task<ImageAsset?> UploadAsync(IFormFile file)
        {
            if (!_validator.Validate(file, out var error))
            {
                _logger.LogWarning("Validation failed: {Error}", error);
                return null;
            }

            try
            {
                await using var stream = file.OpenReadStream();

                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Folder = "app-images",
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
                _logger.LogError(ex, "Upload exception");
                return null;
            }
        }

        public async Task<bool> DeleteAsync(string publicId)
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
    }
}