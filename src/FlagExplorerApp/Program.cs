using System.Text.Json;
using FlagExplorerApp.FlagExplorerApp.Apis;
using FlagExplorerApp.FlagExplorerApp.Services;
using FlagExplorerApp.Middlewares;
using FlagExplorerApp.Services.Configs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:3000") // Update with React frontend URL
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddHttpClient();
builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection("ApiSettings"));
builder.Services.AddScoped<ICountryService, CountryService>();
builder.Services.AddSingleton<ICountryApi, CountryApi>();
builder.Services.AddScoped<IHttpService, HttpService>();
builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
{
    options.SerializerOptions.PropertyNameCaseInsensitive = false;
    options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
});

builder.Services.AddMemoryCache();
builder.Services.AddSingleton<ICacheService, MemoryCacheService>();

var app = builder.Build();

app.UseCors("AllowReactApp");

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    var countryApi = app.Services.GetRequiredService<ICountryApi>();
    countryApi.RegisterRoutes(endpoints);
});

app.Run();