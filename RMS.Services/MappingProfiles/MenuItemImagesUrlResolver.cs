using AutoMapper;
using Microsoft.Extensions.Configuration;
using RMS.Domain.Entities;

namespace RMS.Services.MappingProfiles
{
    public class MenuItemImagesUrlResolver<TDestination> : IValueResolver<MenuItem, TDestination, string>
    {
        private readonly IConfiguration _configuration;

        public MenuItemImagesUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string Resolve(MenuItem source, TDestination destination, string destMember, ResolutionContext context)
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
