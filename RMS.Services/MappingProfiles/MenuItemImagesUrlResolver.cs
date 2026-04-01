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
    public class MenuItemImagesUrlResolver : IValueResolver<MenuItem, MenuItemDTO, List<string>>
    {
        private readonly IConfiguration _configuration;

        public MenuItemImagesUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public List<string> Resolve(MenuItem source, MenuItemDTO destination, List<string> destMember, ResolutionContext context)
        {
            List<string> result = new List<string>();
            var baseUrl = _configuration.GetSection("URLs")["BaseUrl"];
            if (source.ImageUrl is null) return [];
            foreach (var item in source.ImageUrl)
            {
                if(item is null) continue;
                if (item.ImageUrl.StartsWith("http")) 
                    result.Add(item.ImageUrl);

                if(baseUrl is null) continue;
                var imageUrl = $"{baseUrl}{item.ImageUrl}";
                result.Add(imageUrl);
            }
            return result;
        }
    }
}
