using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using RMS.ServicesAbstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Persistence
{
    public class ImageService : IImageService
    {
        private readonly IWebHostEnvironment _env;

        public ImageService(IWebHostEnvironment env)
        {
            _env = env;
        }
        #region Helper Methods
        private string GetImageFolderPath()
        {
            return Path.Combine(_env.WebRootPath, "Images");
        }
        #endregion
        
        public async Task DeleteImageAsync(string? imagePath)
        {
            if (string.IsNullOrEmpty(imagePath)) return;

            var folderPath = GetImageFolderPath();

            var fullPath = Path.Combine(folderPath, Path.GetFileName(imagePath));

            if (File.Exists(fullPath))
                File.Delete(fullPath);
        }

        public async Task<string?> ReplaceImageAsync(IFormFile? file, string? oldImagePath)
        {
            if (file == null) return oldImagePath;

            await DeleteImageAsync(oldImagePath);

            return await UploadImageAsync(file);
        }

        public async Task<string?> UploadImageAsync(IFormFile? file)
        {
            if (file == null) return null;
            // Validate Extension
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            var extension = Path.GetExtension(file.FileName).ToLower();

            if (!allowedExtensions.Contains(extension))
                throw new Exception("Invalid file type. Only jpg, jpeg, png, webp allowed.");

            // Validate Size (2MB)
            if (file.Length > 2 * 1024 * 1024)
                throw new Exception("File size must not exceed 2MB.");

            var folderPath = GetImageFolderPath();

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            var fileName = Guid.NewGuid() + extension;

            var filePath = Path.Combine(folderPath, fileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);

            return $"Images/{fileName}";
        }
    }
    
}
