using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.ResponseCompression;
using System.IO.Compression;
using Serilog;
using AspNetCoreRateLimit;
using System;

var builder = WebApplication.CreateBuilder(args);

// ✅ Konfigurasi Serilog untuk logging
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();
builder.Host.UseSerilog();

// ✅ Ambil konfigurasi dari appsettings.json
var configuration = builder.Configuration;
var dbConnection = configuration.GetValue<string>("DatabaseSettings:ConnectionString");

if (string.IsNullOrWhiteSpace(dbConnection))
{
    Log.Fatal("Connection string tidak ditemukan di appsettings.json!");
    throw new InvalidOperationException("Connection string tidak ditemukan di appsettings.json!");
}

// ✅ Tambahkan DbContext untuk PostgreSQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(dbConnection));

// ✅ Response Compression (Gzip)
builder.Services.Configure<GzipCompressionProviderOptions>(options =>
{
    options.Level = CompressionLevel.Fastest;
});
builder.Services.AddResponseCompression(options =>
{
    options.Providers.Add<GzipCompressionProvider>();
});

// ✅ Middleware Rate Limiting
builder.Services.AddMemoryCache();
builder.Services.Configure<IpRateLimitOptions>(configuration.GetSection("IpRateLimiting"));
builder.Services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
builder.Services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
builder.Services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();

// ✅ Tambahkan CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

// ✅ Tambahkan Exception Handling Middleware
//builder.Services.AddExceptionHandler();

// 🔹 Tambahkan services controller
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ✅ Gunakan Response Compression
app.UseResponseCompression();

// ✅ Gunakan Middleware Rate Limiting
app.UseIpRateLimiting();

// ✅ Gunakan HSTS di Production
if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

// ✅ Gunakan HTTPS Redirection
app.UseHttpsRedirection();

// ✅ Gunakan Middleware Exception Handling
app.UseExceptionHandler("/error"); // Endpoint untuk error handling

// ✅ Gunakan CORS
app.UseCors("AllowAll");

app.UseRouting();
app.UseAuthorization();
app.MapControllers();

// ✅ Tambahkan Swagger di Development Mode
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();
