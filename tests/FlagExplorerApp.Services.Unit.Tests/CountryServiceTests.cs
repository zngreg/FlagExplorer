using AutoMapper;
using FlagExplorerApp.FlagExplorerApp.Services;
using FlagExplorerApp.Services.Configs;
using FlagExplorerApp.Services.Models;
using Microsoft.Extensions.Options;
using Moq;

namespace FlagExplorerApp.Tests.Services
{
    [TestFixture]
    public class CountryServiceTests
    {
        private Mock<IHttpService> _mockHttpService;
        private IMapper _mapper;
        private IOptions<ApiSettings> _apiSettings;
        private Mock<ICacheService> _mockCacheService;
        private CountryService _countryService;

        [SetUp]
        public void Setup()
        {
            _mockHttpService = new Mock<IHttpService>();
            _mockCacheService = new Mock<ICacheService>();

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            _mapper = mappingConfig.CreateMapper();

            _apiSettings = Options.Create(new ApiSettings { CountryApiBaseUrl = "https://example.com/api" });

            _countryService = new CountryService(_mockHttpService.Object, _mapper, _apiSettings, _mockCacheService.Object);
        }

        [Test]
        public async Task GetAllCountriesAsync_ShouldReturnCachedCountries_WhenCacheExists()
        {
            // Arrange
            var cachedCountries = new List<CountryDetails>
            {
                new CountryDetails { Name = "South Africa", Flag = "https://example.com/flags/za.svg", Population = 122334432, Capital = "Bloem, PTA, CT" },
                new CountryDetails { Name = "United States", Flag = "https://example.com/flags/us.svg", Population = 331449281, Capital = "Washington D.C." }
            };

            _mockCacheService.Setup(s => s.TryGetValue(It.IsAny<string>(), out It.Ref<object>.IsAny))
                .Callback((object key, out object value) => { value = cachedCountries; })
                .Returns(true);

            // Act
            var result = await _countryService.GetAllCountriesAsync();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task GetAllCountriesAsync_ShouldFetchFromApi_WhenCacheIsEmpty()
        {
            // Arrange
            _mockCacheService.Setup(s => s.TryGetValue("AllCountries", out It.Ref<IEnumerable<Country>>.IsAny)).Returns(false);

            var apiResponse = new List<CountryInfo>
            {
                new CountryInfo { Name = new NameInfo { Common = "South Africa" }, Flags = new FlagInfo { Svg = "https://example.com/flags/za.svg" } },
                new CountryInfo { Name = new NameInfo { Common = "United States" }, Flags = new FlagInfo { Svg = "https://example.com/flags/us.svg" } }
            };
            _mockHttpService.Setup(s => s.GetAsync<List<CountryInfo>>(It.IsAny<string>())).ReturnsAsync(apiResponse);

            // Act
            var result = await _countryService.GetAllCountriesAsync();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task GetCountryByNameAsync_ShouldReturnCachedCountry_WhenCacheExists()
        {
            // Arrange
            var cachedCountries = new List<CountryDetails>
            {
                new CountryDetails { Name = "South Africa", Flag = "https://example.com/flags/za.svg", Population = 122334432, Capital = "Bloem, PTA, CT" },
                new CountryDetails { Name = "United States", Flag = "https://example.com/flags/us.svg", Population = 331449281, Capital = "Washington D.C." }
            };

            _mockCacheService.Setup(s => s.TryGetValue(It.IsAny<string>(), out It.Ref<object>.IsAny))
                .Callback((object key, out object value) => { value = cachedCountries; })
                .Returns(true);

            // Act
            var result = await _countryService.GetCountryByNameAsync("South Africa");

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo("South Africa"));
        }

        [Test]
        public async Task GetCountryByNameAsync_ShouldFetchFromApi_WhenCacheIsEmpty()
        {
            // Arrange
            _mockCacheService.Setup(s => s.TryGetValue("South Africa", out It.Ref<CountryDetails>.IsAny)).Returns(false);

            var apiResponse = new List<CountryInfo>
            {
                new CountryInfo { Name = new NameInfo { Common = "South Africa" }, Population = 122334432, Capital = new List<string> { "Bloem", "PTA", "CT" }, Flags = new FlagInfo { Svg = "https://example.com/flags/za.svg" } }
            };
            _mockHttpService.Setup(s => s.GetAsync<List<CountryInfo>>(It.IsAny<string>())).ReturnsAsync(apiResponse);

            // Act
            var result = await _countryService.GetCountryByNameAsync("South Africa");

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo("South Africa"));
        }

        [Test]
        public void Constructor_ShouldThrowException_WhenApiBaseUrlIsNull()
        {
            // Arrange
            var invalidApiSettings = Options.Create<ApiSettings>(null);

            // Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() =>
                new CountryService(_mockHttpService.Object, _mapper, invalidApiSettings, _mockCacheService.Object)
            );

            Assert.That(ex.Message, Does.Contain("API URL is missing"));
        }
    }
}