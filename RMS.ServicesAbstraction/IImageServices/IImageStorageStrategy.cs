using Microsoft.AspNetCore.Http;
using RMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.ServicesAbstraction.IImageServices
{
    public interface IImageStorageStrategy
    {
        Task<ImageAsset?> UploadAsync(IFormFile file);
        Task<bool> DeleteAsync(string publicId);
    }
}
