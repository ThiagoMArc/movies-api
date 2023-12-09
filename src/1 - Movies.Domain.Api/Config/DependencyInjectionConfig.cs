using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Movies.Domain.CrossCutting.Configuration;
using Movies.Domain.Infra.Context;
using Movies.Domain.Infra.Repositories;
using Movies.Domain.Repositories;
using Movies.Domain.Services;

namespace Movies.Domain.Api.Config;

public static class DependencyInjectionConfig
{
    public static IServiceCollection ResolveDependencies(this WebApplicationBuilder builder)
    {
        ConfigureDatabase(builder);
        ConfigureMediator(builder);
        ConfigureRedis(builder);
        ConfigureSwagger(builder);
        ConfigureApiVersioning(builder);

        InjectRepositories(builder);
        InjectCache(builder);

        return builder.Services;
    }

    private static void ConfigureDatabase(WebApplicationBuilder builder)
    {
        builder.Services.Configure<DbSettings>(builder.Configuration.GetSection(nameof(DbSettings)));

        builder.Services.AddSingleton<AppDbContext>(serviceProvider =>
        {
            var settings = serviceProvider.GetRequiredService<IOptions<DbSettings>>().Value;
            return new AppDbContext(settings.ConnectionString, settings.DatabaseName);
        });
    }

    private static void ConfigureMediator(WebApplicationBuilder builder)
    {
        builder.Services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(AppDomain.CurrentDomain.Load("Movies.Domain"));
        });
    }

    private static void ConfigureRedis(WebApplicationBuilder builder)
    {
        builder.Services.AddStackExchangeRedisCache(options => { options.Configuration = builder.Configuration["RedisUrl"]; });
    }

    private static void ConfigureSwagger(WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo()
            {
                Title = "Movies Api",
                Description = "Movies Catalogue Api"
            });
        });
    }

    private static void ConfigureApiVersioning(WebApplicationBuilder builder)
    {
        builder.Services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.ReportApiVersions = true;
            options.AssumeDefaultVersionWhenUnspecified = true;
        })
        .AddVersionedApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });
    }

    private static void InjectRepositories(WebApplicationBuilder builder)
    {
        builder.Services.AddTransient<IMovieRepository, MovieRepository>();
    }

    private static void InjectCache(WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IDistributedCache, RedisCache>();
        builder.Services.AddSingleton(typeof(ICache<>), typeof(DistributedCache<>));
    }
}
