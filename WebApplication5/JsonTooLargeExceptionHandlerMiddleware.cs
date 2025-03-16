using Microsoft.AspNetCore.Http;
using System;
using System.Text.Json;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

public class JsonTooLargeExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly long _maxJsonSize;

    public JsonTooLargeExceptionHandlerMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        _next = next;
        _maxJsonSize = configuration.GetValue<long>("MaxJsonSize", 1_000_000);
    }

    public async Task Invoke(HttpContext httpContext)
    {
        try
        {
            httpContext.Request.EnableBuffering();

            if (httpContext.Request.ContentLength.HasValue && httpContext.Request.ContentLength > _maxJsonSize)
            {
                throw new BadHttpRequestException("The JSON value is too large.", StatusCodes.Status413PayloadTooLarge);
            }

            await _next(httpContext);

            if (httpContext.Response.Body.Length > _maxJsonSize)
            {
                throw new BadHttpRequestException("Response JSON too large", StatusCodes.Status413PayloadTooLarge);
            }
        }
        catch (BadHttpRequestException ex) when (ex.Message.Contains("too large"))
        {
            await HandleExceptionAsync(httpContext, ex);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext httpContext, Exception ex)
    {
        httpContext.Response.StatusCode = StatusCodes.Status413PayloadTooLarge;
        httpContext.Response.ContentType = "application/problem+json";

        var problemDetails = new
        {
            type = "https://httpstatuses.com/413",
            title = "Payload Too Large",
            status = 413,
            detail = "The JSON payload is too large and cannot be processed.",
            instance = httpContext.Request.Path
        };

        var json = JsonSerializer.Serialize(problemDetails, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await httpContext.Response.WriteAsync(json);
    }
}
