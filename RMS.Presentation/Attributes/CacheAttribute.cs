using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using RMS.ServicesAbstraction.IServices.ICacheServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Presentation.Attributes
{
    public class CacheAttribute : ActionFilterAttribute
    {
        private readonly int _expirationInMinutes;
        public CacheAttribute(int expirationInMinutes = 5)
        {
            _expirationInMinutes = expirationInMinutes;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var cacheService = context.HttpContext.RequestServices.GetRequiredService<ICacheService>();

            var cacheKey = CreateCacheKey(context.HttpContext.Request);

            var cacheValue = await cacheService.GetAsync(cacheKey);
            if (cacheValue is not null)
            {
                context.Result = new ContentResult()
                {
                    Content = cacheValue,
                    ContentType = "application/json",
                    StatusCode = StatusCodes.Status200OK
                };
                return;
            }
            var executedContext = await next.Invoke(); 
            if (executedContext.Result is OkObjectResult result)
            {
                await cacheService.SetAsync(cacheKey, result.Value!, TimeSpan.FromMinutes(_expirationInMinutes));
            }
        }

        private string CreateCacheKey(HttpRequest request)
        {
            StringBuilder key = new StringBuilder();
            key.Append(request.Path); 
            foreach (var item in request.Query.OrderBy(X => X.Key))
                key.Append($"|{item.Key}-{item.Value}");
            return key.ToString();
        }

    }
}
