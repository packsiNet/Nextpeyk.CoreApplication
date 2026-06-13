using System.Text;
using CoreApplication.Application.Common.Interfaces;
using CoreApplication.Infrastructure.Auth;
using CoreApplication.Infrastructure.Persistence;
using CoreApplication.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace CoreApplication.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                sql =>
                {
                    sql.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                    sql.UseNetTopologySuite();
                }));

        services.AddScoped<IApplicationDbContext>(sp =>
            sp.GetRequiredService<ApplicationDbContext>());

        services.AddScoped<IDateTimeService, DateTimeService>();
        services.AddScoped<IPasswordService, PasswordService>();
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<ISmsSender, ConsoleSmsStub>();
        services.AddScoped<ISlaCalculationService, SlaCalculationService>();
        services.AddHostedService<SlaSnapshotBackgroundService>();

        services.AddScoped<IRouteSimplificationService, DouglasPeuckerService>();
        services.AddScoped<IMapMatchingService, OsrmMapMatchingService>();
        services.AddHttpClient("osrm");

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!)),
                    ValidateIssuer = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = configuration["Jwt:Audience"],
                    ValidateLifetime = true
                };
                // SignalR sends JWT via query string (WebSocket can't use headers)
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var token = context.Request.Query["access_token"];
                        if (!string.IsNullOrEmpty(token) &&
                            context.HttpContext.Request.Path.StartsWithSegments("/hubs/location"))
                        {
                            context.Token = token;
                        }
                        return Task.CompletedTask;
                    }
                };
            })
            .AddScheme<ApiKeyAuthenticationOptions, ApiKeyAuthenticationHandler>(
                ApiKeyDefaults.AuthenticationScheme, _ => { });

        return services;
    }
}
