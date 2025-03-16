using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;



    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class GzipMiddleware
    {
        private readonly RequestDelegate _next;

        public GzipMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Headers.ContainsKey("Content-Encoding") &&
          context.Request.Headers["Content-Encoding"].ToString().ToLower().Contains("gzip"))
            {
                using (var decompressionStream = new GZipStream(context.Request.Body, CompressionMode.Compress))
                {
                    using (var reader = new StreamReader(decompressionStream))
                    {
                        var decompressedBody = await reader.ReadToEndAsync();
                        context.Request.Body = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(decompressedBody));
                    }
                }
            }

            await _next(context);

            //return _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class GzipMiddlewareExtensions
    {
        public static IApplicationBuilder UseGzipMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<GzipMiddleware>();
        }
    }

