using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next.Invoke(httpContext);
            await HandleNotFoundEndPointAsync(httpContext);
        }
        //catch (KeyNotFoundException ex)
        //{

        //    context.Response.StatusCode = 404;

        //    var message = _localizer[ex.Message];

        //    await context.Response.WriteAsJsonAsync(new
        //    {
        //        message
        //    });
        //}
        //catch (Exception ex)
        //{
        //    context.Response.StatusCode = 400;

        //    var message = _localizer[ex.Message];

        //    await context.Response.WriteAsJsonAsync(new
        //    {
        //        message
        //    });
        //}

        catch (Exception ex)
        {
            _logger.LogError(ex, "Something went wrong: {Message}", ex.Message);
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext httpContext, Exception ex)
    {
      

      
        var localizedDetail = _localizer[ex.Message].ResourceNotFound? ex.Message : _localizer[ex.Message].Value;

        var problem = new ProblemDetails
        {
            Title = "Error While Processing The HTTP Request",
            Detail = localizedDetail,
            Instance = httpContext.Request.Path,
            Status = ex switch
            {
                NotFoundException => (StatusCodes.Status404NotFound),
                UnauthorizedAccessException => (StatusCodes.Status401Unauthorized),
                InvalidOperationException => (StatusCodes.Status400BadRequest),
                _ => (StatusCodes.Status500InternalServerError)
            }
        };

        httpContext.Response.StatusCode = problem.Status.Value;
        httpContext.Response.ContentType = "application/json";

        await httpContext.Response.WriteAsJsonAsync(problem);
    }


    private async Task HandleNotFoundEndPointAsync(HttpContext httpContext)
    {
        if (httpContext.Response.StatusCode == StatusCodes.Status404NotFound && !httpContext.Response.HasStarted)
        {
            var response = new ProblemDetails
            {
                Title = "Error White Processing The HTTP Request - EndPoint Not Found",
                Detail = $"Endpoint '{httpContext.Request.Path}' not found",
                Status = StatusCodes.Status404NotFound,
                Instance = httpContext.Request.Path
            };

            httpContext.Response.ContentType = "application/json";
            await httpContext.Response.WriteAsJsonAsync(response);
        }
    }





}






































