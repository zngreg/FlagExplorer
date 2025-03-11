using AutoMapper;
using FlagExplorerApp.Services.Models;
using Microsoft.Extensions.Options;

namespace FlagExplorerApp.FlagExplorerApp.Services
{
    public class CountryService : ICountryService
    {
        private readonly IHttpService _httpService;
        private readonly IMapper _mapper;
        private readonly string _apiBaseUrl;

        public CountryService(IHttpService httpService, IMapper mapper, IOptions<ApiSettings> apiSettings)
        {
            _httpService = httpService;
            _mapper = mapper;
            _apiBaseUrl = apiSettings?.Value?.CountryApiBaseUrl ?? throw new ArgumentNullException("API URL is missing");
        }

        public async Task<IEnumerable<Country>> GetAllCountriesAsync()
        {
            var response = await _httpService.GetAsync<List<CountryInfo>>($"{_apiBaseUrl}/all");
            return response != null ? _mapper.Map<IEnumerable<Country>>(response) : Enumerable.Empty<Country>();
        }

        public async Task<CountryDetails?> GetCountryByNameAsync(string name)
        {
            var response = await _httpService.GetAsync<List<CountryInfo>>($"{_apiBaseUrl}/name/{name}?fullText=true");
            var country = response?.SingleOrDefault();
            return country is not null ? _mapper.Map<CountryDetails>(country) : null;
        }
    }
}