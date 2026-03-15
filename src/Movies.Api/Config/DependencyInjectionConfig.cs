using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Movies.Domain.Api.Behaviors;
using Movies.Domain.Api.MIddlewares;
using Movies.Domain.CrossCutting.Configuration;
using Movies.Domain.Infra.Context;
using Movies.Domain.Infra.Repositories;
using Movies.Domain.Repositories;
using Movies.Domain.Services;
using Keycloak.AuthServices.Authentication;
using Keycloak.AuthServices.Authorization;
using Keycloak.AuthServices.Sdk;
using Movies.Domain.Queries.v1.GetMovies;
using Movies.Domain.Queries.GetMovies.v1.GetMovies;
using Movies.Domain.Queries.v1.GetMoviesById;
using Movies.Domain.Commands.v1.CreateMovie;
using Movies.Domain.Commands.v1.UpdateMovie;
using Movies.Domain.Commands.v1.DeleteMovie;
using Movies.Domain.Commands.UpdateMovie.v1.UpdateMovie;

namespace Movies.Domain.Api.Config;

public static class DependencyInjectionConfig
{
    public static IServiceCollection ResolveDependencies(this WebApplicationBuilder builder)
    {
        ConfigureAuthentication(builder);
        ConfigureDatabase(builder);
        ConfigureValidators(builder);
        ConfigureMediator(builder);
        RegisterExceptionMiddleware(builder);
        ConfigureRedis(builder);
        ConfigureSwagger(builder);
        ConfigureApiVersioning(builder);

        InjectRepositories(builder);
        InjectCache(builder);

        return builder.Services;
    }

    private static void ConfigureAuthentication(WebApplicationBuilder builder)
    {
        builder.Services.AddKeycloakWebApiAuthentication(builder.Configuration);
        builder.Services.AddKeycloakAuthorization(builder.Configuration);

        var adminClientOptions = builder
                            .Configuration
                            .GetSection(KeycloakAdminClientOptions.Section)
                            .Get<KeycloakAdminClientOptions>() ?? new KeycloakAdminClientOptions();

        builder.Services.AddKeycloakAdminHttpClient(adminClientOptions);
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

    private static void ConfigureValidators(WebApplicationBuilder builder)
    {
        builder.Services.AddTransient<IValidator<GetMoviesQuery>, GetMoviesQueryValidator>();
        builder.Services.AddTransient<IValidator<GetMovieByIdQuery>, GetMovieByIdValidator>();
        builder.Services.AddTransient<IValidator<CreateMovieCommand>, CreateMovieCommandValidator>();
        builder.Services.AddTransient<IValidator<UpdateMovieCommand>, updateMovieCommandValidator>();
        builder.Services.AddTransient<IValidator<DeleteMovieCommand>, DeleteMovieCommandValidator>();
    }

    private static void ConfigureMediator(WebApplicationBuilder builder)
    {
        var domainAssembly = AppDomain.CurrentDomain.Load("Movies.Domain");

        builder.Services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(domainAssembly);
        });

        builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
    }

    private static void RegisterExceptionMiddleware(WebApplicationBuilder builder)
    {
        builder.Services.AddTransient<ExceptionHandlingMiddleware>();
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
