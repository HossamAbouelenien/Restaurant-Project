using AutoMapper;
using Microsoft.Extensions.Configuration;
using RMS.Domain.Entities;
using RMS.Shared.DTOs.MenuItemsDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Services.MappingProfiles
{
    public class MenuItemImagesUrlResolver : IValueResolver<MenuItem, MenuItemDTO, string>
    {
        private readonly IConfiguration _configuration;

        public MenuItemImagesUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string Resolve(MenuItem source, MenuItemDTO destination, string destMember, ResolutionContext context)
        {
            if (string.IsNullOrEmpty(source.ImageUrl))
                return string.Empty;

            if (source.ImageUrl.StartsWith("http"))
                return source.ImageUrl;

            var baseUrl = _configuration.GetSection("URLs")["BaseUrl"];
            if (string.IsNullOrEmpty(baseUrl))
                return string.Empty;
            var imageUrl = $"{baseUrl}{source.ImageUrl}";
            return imageUrl;
        }
    }
}
