using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using FlagExplorerApp.Middlewares;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace FlagExplorerApp.Tests.Middlewares
{
    [TestFixture]
    public class ErrorHandlingMiddlewareTests
    {
        private Mock<RequestDelegate> _mockNext;
        private Mock<ILogger<ErrorHandlingMiddleware>> _mockLogger;
        private ErrorHandlingMiddleware _middleware;

        [SetUp]
        public void Setup()
        {
            _mockNext = new Mock<RequestDelegate>();
            _mockLogger = new Mock<ILogger<ErrorHandlingMiddleware>>();
            _middleware = new ErrorHandlingMiddleware(_mockNext.Object, _mockLogger.Object);
        }

        [Test]
        public async Task InvokeAsync_ShouldCallNextMiddleware_WhenNoExceptionOccurs()
        {
            // Arrange
            var context = new DefaultHttpContext();
            _mockNext.Setup(next => next(context)).Returns(Task.CompletedTask);

            // Act
            await _middleware.InvokeAsync(context);

            // Assert
            Assert.That(context.Response.StatusCode, Is.EqualTo(200));
            _mockNext.Verify(next => next(context), Times.Once);
        }

        [Test]
        public async Task InvokeAsync_ShouldReturnBadRequest_WhenExceptionOccurs()
        {
            // Arrange
            var context = new DefaultHttpContext();
            _mockNext.Setup(next => next(context)).ThrowsAsync(new Exception("Test exception"));

            // Capture response
            await using var responseStream = new MemoryStream();
            context.Response.Body = responseStream;

            // Act
            await _middleware.InvokeAsync(context);

            // Assert
            Assert.That(context.Response.StatusCode, Is.EqualTo(400));
            Assert.That(context.Response.ContentType, Is.EqualTo("application/json; charset=utf-8"));

            // Read response body
            responseStream.Seek(0, SeekOrigin.Begin);
            var responseBody = await new StreamReader(responseStream).ReadToEndAsync();
            var responseObj = JsonSerializer.Deserialize<JsonElement>(responseBody);

            Assert.That(responseObj.TryGetProperty("message", out var messageProp), Is.True);
            Assert.That(messageProp.GetString(), Does.Contain("An unexpected error occurred"));
            Assert.That(messageProp.GetString(), Does.Contain("CorelationId:"));
        }
    }
}