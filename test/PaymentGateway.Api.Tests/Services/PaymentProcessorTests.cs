using System.Net;
using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using Moq.Protected;
using PaymentGateway.Api.Domain;
using PaymentGateway.Api.Services.PaymentsProcessor;

public class PaymentsProcessorTests
{
    private Payment CreateFakePayment() =>
        new Payment("1234567890123456", 12, 2030, "USD", 100, "123");

    private HttpClient CreateHttpClient(HttpStatusCode statusCode, object? content = null)
    {
        var handlerMock = new Mock<HttpMessageHandler>();

        handlerMock
           .Protected()
           .Setup<Task<HttpResponseMessage>>(
               "SendAsync",
               ItExpr.IsAny<HttpRequestMessage>(),
               ItExpr.IsAny<CancellationToken>()
           )
           .ReturnsAsync(() =>
           {
               var response = new HttpResponseMessage(statusCode);
               if (content != null)
               {
                   response.Content = JsonContent.Create(content);
               }
               return response;
           });

        return new HttpClient(handlerMock.Object);
    }

    [Fact]
    public async Task ProcessPaymentAsync_ShouldLogInformation_WhenAuthorized()
    {
        // Arrange
        var payment = CreateFakePayment();
        var responseContent = new
        {
            authorized = true,
            authorization_code = "1234"
        };

        var httpClient = CreateHttpClient(HttpStatusCode.OK, responseContent);
        var loggerMock = new Mock<ILogger<PaymentsProcessor>>();

        var processor = new PaymentsProcessor(httpClient, loggerMock.Object);

        // Act
        var result = await processor.ProcessPaymentAsync(payment, CancellationToken.None);

        // Assert result
        Assert.True(result);

        // Assert logs
        loggerMock.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Sending payment request")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()
            ), Times.Once);

        loggerMock.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Bank simulator responded with status code")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()
            ), Times.Once);

        loggerMock.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Bank response: Authorized=True")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()
            ), Times.Once);
    }

    [Fact]
    public async Task ProcessPaymentAsync_ShouldLogWarning_WhenNonOkStatus()
    {
        // Arrange
        var payment = CreateFakePayment();
        var httpClient = CreateHttpClient(HttpStatusCode.ServiceUnavailable);
        var loggerMock = new Mock<ILogger<PaymentsProcessor>>();

        var processor = new PaymentsProcessor(httpClient, loggerMock.Object);

        // Act
        var result = await processor.ProcessPaymentAsync(payment, CancellationToken.None);

        // Assert
        Assert.False(result);

        // Verify warning log
        loggerMock.Verify(
            x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Bank simulator returned a non-success status code")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()
            ), Times.Once);
    }

    [Fact]
    public async Task ProcessPaymentAsync_ShouldLogError_WhenHttpRequestException()
    {
        // Arrange
        var payment = CreateFakePayment();

        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ThrowsAsync(new HttpRequestException("Bank unreachable"));

        var httpClient = new HttpClient(handlerMock.Object);
        var loggerMock = new Mock<ILogger<PaymentsProcessor>>();

        var processor = new PaymentsProcessor(httpClient, loggerMock.Object);

        // Act
        var result = await processor.ProcessPaymentAsync(payment, CancellationToken.None);

        // Assert
        Assert.False(result);

        // Verify error log
        loggerMock.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Failed to reach bank simulator")),
                It.IsAny<HttpRequestException>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()
            ), Times.Once);
    }
}
