using FlagExplorerApp.Services.Models;

namespace FlagExplorerApp.FlagExplorerApp.Services
{
    public interface ICountryService
    {
        Task<IEnumerable<Country>> GetAllCountriesAsync();
        Task<CountryDetails?> GetCountryByNameAsync(string name);
    }
}