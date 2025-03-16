using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO.Compression;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.Configure<GzipCompressionProviderOptions>(options =>
        {
            options.Level = CompressionLevel.Optimal;
        });

        services.AddResponseCompression(options =>
        {
            options.Providers.Add<GzipCompressionProvider>();
        });

        services.AddControllers();

        // Tambahkan Middleware Request Size Limit
        services.AddSingleton<RequestSizeLimitMiddleware>();

        // Konfigurasi CORS (Hanya izinkan domain tertentu)
        services.AddCors(options =>
        {
            options.AddPolicy("AllowedOrigins",
                builder =>
                {
                    builder.WithOrigins(Configuration.GetSection("AllowedHosts").Get<string[]>())
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/error");
        }

        app.UseResponseCompression();
        app.UseCors("AllowedOrigins");

        app.UseRouting();
        app.UseAuthorization();

        //app.UseMiddleware<RequestSizeLimitMiddleware>();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
