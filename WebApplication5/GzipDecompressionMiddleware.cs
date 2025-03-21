﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace netCoreApiSyspro
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class GzipDecompressionMiddleware
    {
        private readonly RequestDelegate _next;

        public GzipDecompressionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext)
        {

            return _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class GzipDecompressionMiddlewareExtensions
    {
        public static IApplicationBuilder UseGzipDecompressionMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<GzipDecompressionMiddleware>();
        }
    }
}
