using System.Net;
using System.Text.Json;
using FlagExplorerApp.FlagExplorerApp.Services;
using FlagExplorerApp.Services.Models;
using Moq;
using Moq.Protected;

namespace FlagExplorerApp.Tests.Services
{
    [TestFixture]
    public class HttpServiceTests
    {
        private Mock<HttpMessageHandler> _mockHttpMessageHandler;
        private HttpClient _httpClient;
        private HttpService _httpService;

        [SetUp]
        public void Setup()
        {
            _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_mockHttpMessageHandler.Object)
            {
                BaseAddress = new System.Uri("https://example.com")
            };
            _httpService = new HttpService(_httpClient);
        }

        [TearDown]
        public void TearDown()
        {
            _httpClient.Dispose();
        }

        [Test]
        public async Task GetAsync_ShouldReturnDeserializedObject_WhenApiReturnsSuccess()
        {
            // Arrange
            var expectedData = new CountryInfo
            {
                Name = new NameInfo { Common = "South Africa" },
                Flag = "https://example.com/flags/za.png",
                Population = 59308690,
                Capital = new List<string> { "Pretoria", "Bloemfontein", "Cape Town" },
                Flags = new FlagInfo
                {
                    Png = "https://example.com/flags/za.png",
                    Svg = "https://example.com/flags/za.svg",
                    Alt = "Flag of South Africa"
                }
            };
            var jsonResponse = JsonSerializer.Serialize(expectedData);
            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(jsonResponse)
            };
            responseMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            _mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(responseMessage);

            // Act
            var result = await _httpService.GetAsync<CountryInfo>("https://example.com/countries/ZA");

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name.Common, Is.EqualTo("South Africa"));
            Assert.That(result.Flag, Is.EqualTo("https://example.com/flags/za.png"));
            Assert.That(result.Population, Is.EqualTo(59308690));
            Assert.That(result.Flags.Alt, Is.EqualTo("Flag of South Africa"));
            Assert.That(result.Flags.Png, Is.EqualTo("https://example.com/flags/za.png"));
            Assert.That(result.Flags.Svg, Is.EqualTo("https://example.com/flags/za.svg"));
            Assert.That(result.Capital.Count, Is.EqualTo(3));
            Assert.That(result.Capital.Contains("Pretoria"), Is.True);
            Assert.That(result.Capital.Contains("Bloemfontein"), Is.True);
            Assert.That(result.Capital.Contains("Cape Town"), Is.True);
        }

        [Test]
        public async Task GetAsync_ShouldReturnNull_WhenApiReturnsNotFound()
        {
            // Arrange
            var responseMessage = new HttpResponseMessage(HttpStatusCode.NotFound);

            _mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(responseMessage);

            // Act
            Assert.ThrowsAsync<HttpRequestException>(async () => await _httpService.GetAsync<dynamic>("https://example.com/countries/unknown"));
        }

        [Test]
        public async Task GetAsync_ShouldThrowException_WhenApiCallFails()
        {
            // Arrange
            _mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ThrowsAsync(new HttpRequestException("Network error"));

            // Act & Assert
            var exception = Assert.ThrowsAsync<HttpRequestException>(async () =>
                await _httpService.GetAsync<dynamic>("https://example.com/countries/error")
            );

            Assert.That(exception.Message, Is.EqualTo("Network error"));
        }
    }
}
