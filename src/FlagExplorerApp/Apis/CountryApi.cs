using FlagExplorerApp.FlagExplorerApp.Services;

namespace FlagExplorerApp.FlagExplorerApp.Apis
{
    public class CountryApi : ICountryApi
    {
        public void RegisterRoutes(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet("/countries", async (ICountryService countryService) =>
            {
                var countries = await countryService.GetAllCountriesAsync();
                return Results.Ok(countries);
            });

            endpoints.MapGet("/countries/{name}", async (ICountryService countryService, string name) =>
            {
                var country = await countryService.GetCountryByNameAsync(name);
                return country is not null ? Results.Ok(country) : Results.NotFound("Country not found");
            });
        }
    }
}