using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Movies.Domain.CrossCutting.Configuration;
using Movies.Domain.Infra.Context;
using Movies.Domain.Infra.Repositories;
using Movies.Domain.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<DbSettings>(builder.Configuration.GetSection(nameof(DbSettings)));

builder.Services.AddSingleton<AppDbContext>(serviceProvider =>
{
    var settings = serviceProvider.GetRequiredService<IOptions<DbSettings>>().Value;
    return new AppDbContext(settings.ConnectionString, settings.DatabaseName);
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => 
{
    c.SwaggerDoc("v1", new OpenApiInfo()
                       {
                        Title = "Movies Api",
                        Description = "Movies Catalogue Api"
                       }
                );
});

builder.Services.AddMediatR(cfg => {
    cfg.RegisterServicesFromAssembly(AppDomain.CurrentDomain.Load("Movies.Domain"));
});

builder.Services.AddTransient<IMovieRepository, MovieRepository>();

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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
