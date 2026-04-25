using Microsoft.Extensions.Localization;
using RMS.Shared.SharedResources;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IStringLocalizer<SharedResources> _localizer;

    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next,
        IStringLocalizer<SharedResources> localizer,
        ILogger<ExceptionMiddleware> logger
        )
    {
        _next = next;
        _localizer = localizer;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (KeyNotFoundException ex)
        {
            
            context.Response.StatusCode = 404;

            var message = _localizer[ex.Message];

            await context.Response.WriteAsJsonAsync(new
            {
                message
            });
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = 400;

            var message = _localizer[ex.Message];

            await context.Response.WriteAsJsonAsync(new
            {
                message
            });
        }
    }
}