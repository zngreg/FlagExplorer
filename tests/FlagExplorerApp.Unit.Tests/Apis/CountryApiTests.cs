using FlagExplorerApp.FlagExplorerApp.Apis;
using FlagExplorerApp.FlagExplorerApp.Services;
using FlagExplorerApp.Services.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Newtonsoft.Json;
using System.Net;

namespace FlagExplorerApp.Unit.Tests.Apis
{
    [TestFixture]
    public class CountryApiTests
    {
        private Mock<ICountryService> _mockCountryService;
        private HttpClient _client;
        private TestServer _server;

        [SetUp]
        public void Setup()
        {
            _mockCountryService = new Mock<ICountryService>();

            var webHostBuilder = new WebHostBuilder()
                .ConfigureServices(services =>
                {
                    services.AddRouting(); // 🔥 Fix: Add routing services
                    services.AddSingleton(_mockCountryService.Object);
                })
                .Configure(app =>
                {
                    app.UseRouting();
                    app.UseEndpoints(endpoints =>
                    {
                        var countryApi = new CountryApi();
                        countryApi.RegisterRoutes(endpoints);
                    });
                });

            _server = new TestServer(webHostBuilder);
            _client = _server.CreateClient();
        }

        [TearDown]
        public void TearDown()
        {
            _client.Dispose();
            _server.Dispose();
        }

        [Test]
        public async Task GetAllCountries_ShouldReturnOk_WithCountries()
        {
            // Arrange
            var countries = new List<Country>
            {
                new Country { Name = "Test Name 1", Flag = "Test Flag 1" },
                new Country { Name = "Test Name 2", Flag = "Test Flag 2" }
            };

            _mockCountryService.Setup(service => service.GetAllCountriesAsync())
                               .ReturnsAsync(countries);

            // Act
            var response = await _client.GetAsync("/countries");

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var content = await response.Content.ReadAsStringAsync();
            var deserializedContent = JsonConvert.DeserializeObject<List<Country>>(content);

            Assert.That(deserializedContent, Is.Not.Null);
            Assert.That(deserializedContent.Count, Is.EqualTo(2));
            Assert.That(deserializedContent[0].Name, Is.EqualTo("Test Name 1"));
            Assert.That(deserializedContent[1].Name, Is.EqualTo("Test Name 2"));
        }

        [Test]
        public async Task GetCountryByName_ShouldReturnOk_WithCountry()
        {
            // Arrange
            var countryName = "USA";
            var country = new CountryDetails { Name = countryName, Capital = "Washington, D.C." };
            _mockCountryService.Setup(service => service.GetCountryByNameAsync(countryName))
                               .ReturnsAsync(country);

            // Act
            var response = await _client.GetAsync($"/countries/{countryName}");

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var content = await response.Content.ReadAsStringAsync();
            var deserializedContent = JsonConvert.DeserializeObject<CountryDetails>(content);

            Assert.That(deserializedContent, Is.Not.Null);
            Assert.That(deserializedContent.Name, Is.EqualTo(countryName));
        }

        [Test]
        public async Task GetCountryByName_ShouldReturnNotFound_WhenCountryDoesNotExist()
        {
            // Arrange
            var countryName = "NonExistentCountry";
            _mockCountryService.Setup(service => service.GetCountryByNameAsync(countryName))
                               .ReturnsAsync((CountryDetails)null);

            // Act
            var response = await _client.GetAsync($"/countries/{countryName}");

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));

            var content = await response.Content.ReadAsStringAsync();
            Assert.That(content, Does.Contain("Country not found"));
        }
    }
}