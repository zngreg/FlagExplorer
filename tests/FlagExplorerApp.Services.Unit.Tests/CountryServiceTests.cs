using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FlagExplorerApp.FlagExplorerApp.Services;
using FlagExplorerApp.Services.Configs;
using FlagExplorerApp.Services.Models;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;

namespace FlagExplorerApp.Tests.Services
{
    [TestFixture]
    public class CountryServiceTests
    {
        private Mock<IHttpService> _mockHttpService;
        private IMapper _mapper;
        private IOptions<ApiSettings> _apiSettings;
        private CountryService _countryService;

        [SetUp]
        public void Setup()
        {
            _mockHttpService = new Mock<IHttpService>();

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            _mapper = mappingConfig.CreateMapper();

            _apiSettings = Options.Create(new ApiSettings { CountryApiBaseUrl = "https://example.com/api" });

            _countryService = new CountryService(_mockHttpService.Object, _mapper, _apiSettings);
        }

        [Test]
        public async Task GetAllCountriesAsync_ShouldReturnMappedCountries_WhenApiReturnsData()
        {
            // Arrange
            var apiResponse = new List<CountryInfo>
            {
                new CountryInfo { Name = new NameInfo { Common = "South Africa" }, Flag = "https://example.com/flags/za.png" },
                new CountryInfo { Name = new NameInfo { Common = "United States" }, Flag = "https://example.com/flags/us.png" }
            };
            var mappedCountries = new List<Country>
            {
                new Country { Name = "South Africa", Flag = "https://example.com/flags/za.png" },
                new Country { Name = "United States", Flag = "https://example.com/flags/us.png" }
            };

            _mockHttpService
                .Setup(s => s.GetAsync<List<CountryInfo>>("https://example.com/api/all"))
                .ReturnsAsync(apiResponse);

            // Act
            var result = await _countryService.GetAllCountriesAsync();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(2));
            Assert.That(result.First().Name, Is.EqualTo("South Africa"));
            Assert.That(result.Last().Flag, Is.EqualTo("https://example.com/flags/us.png"));
        }

        [Test]
        public async Task GetAllCountriesAsync_ShouldReturnEmptyList_WhenApiReturnsNull()
        {
            // Arrange
            _mockHttpService
                .Setup(s => s.GetAsync<List<CountryInfo>>("https://example.com/api/all"))
                .ReturnsAsync((List<CountryInfo>?)null);

            // Act
            var result = await _countryService.GetAllCountriesAsync();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Empty);
        }

        [Test]
        public async Task GetCountryByNameAsync_ShouldReturnMappedCountryDetails_WhenApiReturnsData()
        {
            // Arrange
            var apiResponse = new List<CountryInfo>
            {
                new CountryInfo { Name = new NameInfo { Common = "South Africa" }, Population = 122334432, Capital = new List<string> { "Bloem", "PTA", "CT" }, Flag = "https://example.com/flags/za.png" },
            };
            var mappedCountryDetails = new CountryDetails { Name = "South Africa", Population = 122334432, Capital = "Bloem, PTA, CT", Flag = "https://example.com/flags/za.png" };

            _mockHttpService
                .Setup(s => s.GetAsync<List<CountryInfo>>("https://example.com/api/name/South Africa?fullText=true"))
                .ReturnsAsync(apiResponse);

            // Act
            var result = await _countryService.GetCountryByNameAsync("South Africa");

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo("South Africa"));
            Assert.That(result.Flag, Is.EqualTo("https://example.com/flags/za.png"));
            Assert.That(result.Population, Is.EqualTo(122334432));
            Assert.That(result.Capital, Is.EqualTo("Bloem, PTA, CT"));

        }

        [Test]
        public async Task GetCountryByNameAsync_ShouldReturnNull_WhenApiReturnsEmptyList()
        {
            // Arrange
            _mockHttpService
                .Setup(s => s.GetAsync<List<CountryInfo>>("https://example.com/api/name/Narnia?fullText=true"))
                .ReturnsAsync(new List<CountryInfo>());

            // Act
            var result = await _countryService.GetCountryByNameAsync("Narnia");

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public void Constructor_ShouldThrowException_WhenApiBaseUrlIsNull()
        {
            // Arrange
            var invalidApiSettings = Options.Create<ApiSettings>(null);

            // Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() =>
                new CountryService(_mockHttpService.Object, _mapper, invalidApiSettings)
            );

            Assert.That(ex.Message, Does.Contain("API URL is missing"));
        }
    }
}