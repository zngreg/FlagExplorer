using FlagExplorerApp.FlagExplorerApp.Apis;
using FlagExplorerApp.FlagExplorerApp.Services;
using FlagExplorerApp.Middlewares;
using FlagExplorerApp.Services.Configs;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpClient();
builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection("ApiSettings"));
builder.Services.AddScoped<ICountryService, CountryService>();
builder.Services.AddSingleton<ICountryApi, CountryApi>();
builder.Services.AddScoped<IHttpService, HttpService>();
builder.Services.AddAutoMapper(typeof(MappingProfile));

var app = builder.Build();

app.UseMiddleware<ErrorHandlingMiddleware>();

var countryApi = app.Services.GetRequiredService<ICountryApi>();
countryApi.RegisterRoutes(app);

app.Run();