using Microsoft.AspNetCore.HttpOverrides;
using SafariDigital.Api.Builders.Injectors;
using SafariDigital.Core.Application;
using SafariDigital.Database;
using SafariDigital.Database.Context;
using SafariDigital.Services.Authentication;
using SafariDigital.Services.Cache;
using SafariDigital.Services.HttpContext;
using SafariDigital.Services.Jwt;

namespace SafariDigital.Api.Builders;

public static class Builder
{
    public static WebApplication CreateApp(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        return builder
            .AddProjectSettings()
            .ValidateApplicationSettings()
            .ConnectDatabase()
            .ConfigureForwardedHeaders()
            .InjectServices()
            .AddCorsPolicy()
            .AddRateLimiter()
            .AddControllers()
            .AddSwagger()
            .Build();
    }

    private static WebApplicationBuilder InjectServices(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddRepositories()
            .AddHttpContextService()
            .AddJwtService()
            .AddCacheService()
            .AddAuthenticationService();
        return builder;
    }

    private static WebApplicationBuilder AddControllers(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();
        return builder;
    }

    private static WebApplicationBuilder AddCorsPolicy(this WebApplicationBuilder builder)
    {
        var allowedOrigins = builder.Configuration.GetSectionOrThrow<string[]>(EApplicationSetting.CorsAllowedOrigins);
        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(policyBuilder =>
            {
                policyBuilder
                    .WithOrigins(allowedOrigins)
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });
        return builder;
    }

    private static WebApplicationBuilder ConfigureForwardedHeaders(
        this WebApplicationBuilder builder
    )
    {
        builder.Services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            options.KnownNetworks.Clear();
            options.KnownProxies.Clear();
        });
        return builder;
    }
}