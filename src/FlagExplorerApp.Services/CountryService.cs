using AutoMapper;
using FlagExplorerApp.Services.Models;
using Microsoft.Extensions.Options;

namespace FlagExplorerApp.FlagExplorerApp.Services
{
    public class CountryService : ICountryService
    {
        private readonly IHttpService _httpService;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;
        private readonly string _apiBaseUrl;

        public CountryService(
            IHttpService httpService,
            IMapper mapper,
            IOptions<ApiSettings> apiSettings,
            ICacheService cacheService)
        {
            _httpService = httpService;
            _mapper = mapper;
            _cacheService = cacheService;
            _apiBaseUrl = apiSettings?.Value?.CountryApiBaseUrl ?? throw new ArgumentNullException("API URL is missing");
        }

        public async Task<IEnumerable<Country>> GetAllCountriesAsync()
        {
            _cacheService.TryGetValue("AllCountries", out IEnumerable<CountryDetails> countries);
            if (countries is not null)
            {
                return countries as IEnumerable<Country> ?? Enumerable.Empty<Country>();
            }

            var response = await _httpService.GetAsync<List<CountryInfo>>($"{_apiBaseUrl}/all");
            if (response != null)
            {
                _cacheService.Set("AllCountries", _mapper.Map<IEnumerable<CountryDetails>>(response), TimeSpan.FromMinutes(30));
            }

            return response != null ? _mapper.Map<IEnumerable<Country>>(response) : Enumerable.Empty<Country>();
        }

        public async Task<CountryDetails?> GetCountryByNameAsync(string name)
        {
            _cacheService.TryGetValue("AllCountries", out IEnumerable<CountryDetails> countries);
            if (countries is not null && countries.Any(c => c.Name == name))
            {
                return countries.Single(c => c.Name == name);
            }

            var response = await _httpService.GetAsync<List<CountryInfo>>($"{_apiBaseUrl}/name/{name}?fullText=true");
            var responseCountry = response?.SingleOrDefault();

            return responseCountry is not null ? _mapper.Map<CountryDetails>(responseCountry) : null;
        }
    }
}