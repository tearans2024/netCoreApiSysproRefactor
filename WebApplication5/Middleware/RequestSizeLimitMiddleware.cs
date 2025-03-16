using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

public class RequestSizeLimitMiddleware
{
    private readonly RequestDelegate _next;
    private readonly long _maxRequestSize;
    private readonly ILogger<RequestSizeLimitMiddleware> _logger;

    public RequestSizeLimitMiddleware(RequestDelegate next, IConfiguration configuration, ILogger<RequestSizeLimitMiddleware> logger)
    {
        _next = next;
        _logger = logger;
        _maxRequestSize = configuration.GetValue<long>("RequestSizeLimit", 5_242_880); // Default 5MB
    }

    public async Task Invoke(HttpContext context)
    {
        // Pastikan request memiliki Content-Length
        if (context.Request.ContentLength.HasValue && context.Request.ContentLength > _maxRequestSize)
        {
            _logger.LogWarning($"Request size exceeded: {context.Request.ContentLength} bytes (max: {_maxRequestSize} bytes)");

            context.Response.StatusCode = StatusCodes.Status413PayloadTooLarge;
            await context.Response.WriteAsync($"Request size exceeded the limit of {_maxRequestSize} bytes.");
            return;
        }

        await _next(context);
    }
}

// Extension method untuk registrasi middleware
public static class RequestSizeLimitMiddlewareExtensions
{
    public static IApplicationBuilder UseRequestSizeLimitMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<RequestSizeLimitMiddleware>();
    }
}
